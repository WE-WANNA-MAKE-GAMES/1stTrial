using System.Collections.Generic;
using UnityEngine;

public class NeuronElectricAttack : MonoBehaviour
{
    [SerializeField]
    private float attackInterval = 2f;

    [SerializeField]
    private float damage = 2f;

    [SerializeField]
    private float preparationDuration = 0.65f;

    [SerializeField]
    private float dischargeEffectDuration = 0.2f;

    [SerializeField]
    private Color preparationColor = new(0.35f, 0.9f, 1f, 1f);

    [SerializeField]
    private Color dischargeColor = new(0.85f, 1f, 1f, 1f);

    private float timer;
    private float dischargeTimer;
    private Transform player;
    private Neuron ownerNeuron;
    private NeuronConnection connection;
    private SpriteRenderer spriteRenderer;
    private Color originalSpriteColor;

    private void Awake()
    {
        ownerNeuron = GetComponent<Neuron>();
        connection = GetComponent<NeuronConnection>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            originalSpriteColor = spriteRenderer.color;
        }
    }

    private void Update()
    {
        if (player == null)
        {
            GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
            if (playerObject != null)
            {
                player = playerObject.transform;
            }
        }

        timer += Time.deltaTime;
        if (dischargeTimer > 0f)
        {
            dischargeTimer -= Time.deltaTime;
        }

        if (timer >= attackInterval)
        {
            timer = 0f;
            dischargeTimer = dischargeEffectDuration;
            TriggerElectricShock();
            UpdateElectricVisual(0f, true);
            return;
        }

        if (dischargeTimer > 0f)
        {
            UpdateElectricVisual(0f, true);
        }
        else if (timer >= attackInterval - preparationDuration)
        {
            float charge = Mathf.InverseLerp(
                attackInterval - preparationDuration,
                attackInterval,
                timer
            );
            UpdateElectricVisual(charge, false);
        }
        else
        {
            UpdateElectricVisual(0f, false);
        }
    }

    private void UpdateElectricVisual(float charge, bool discharging)
    {
        if (connection == null)
        {
            connection = GetComponent<NeuronConnection>();
        }

        connection?.SetElectricEffect(charge, discharging);

        if (spriteRenderer == null)
        {
            return;
        }

        if (discharging)
        {
            float pulse = 0.7f + 0.3f * Mathf.Sin(Time.time * 70f);
            spriteRenderer.color = Color.Lerp(originalSpriteColor, dischargeColor, pulse);
        }
        else
        {
            float pulse = 0.75f + 0.25f * Mathf.Sin(Time.time * 18f);
            spriteRenderer.color = Color.Lerp(
                originalSpriteColor,
                preparationColor,
                charge * pulse
            );
        }
    }

    private void TriggerElectricShock()
    {
        if (player == null || ownerNeuron == null)
        {
            return;
        }

        List<Neuron> network = GetConnectedNetwork();
        PlayerHealth playerHealth = player.GetComponentInParent<PlayerHealth>();
        if (playerHealth == null)
        {
            return;
        }

        Collider2D[] playerColliders = player.GetComponentsInChildren<Collider2D>();

        foreach (Neuron neuron in network)
        {
            if (neuron == null || !IsTouchingNeuron(playerColliders, neuron))
            {
                continue;
            }

            playerHealth.TakeDamage(damage, neuron.transform);
            return;
        }
    }

    private static bool IsTouchingNeuron(Collider2D[] playerColliders, Neuron neuron)
    {
        if (playerColliders == null || playerColliders.Length == 0)
        {
            return false;
        }

        Collider2D[] neuronColliders = neuron.GetComponents<Collider2D>();
        foreach (Collider2D neuronCollider in neuronColliders)
        {
            if (neuronCollider == null || !neuronCollider.enabled)
            {
                continue;
            }

            foreach (Collider2D playerCollider in playerColliders)
            {
                if (playerCollider == null || !playerCollider.enabled)
                {
                    continue;
                }

                ColliderDistance2D distance = playerCollider.Distance(neuronCollider);
                if (distance.isOverlapped || distance.distance <= 0f)
                {
                    return true;
                }
            }
        }

        return false;
    }

    private List<Neuron> GetConnectedNetwork()
    {
        List<Neuron> result = new();
        if (ownerNeuron == null)
        {
            ownerNeuron = GetComponent<Neuron>();
        }

        if (ownerNeuron == null)
        {
            return result;
        }

        Queue<Neuron> queue = new();
        HashSet<Neuron> visited = new();
        queue.Enqueue(ownerNeuron);

        while (queue.Count > 0)
        {
            Neuron current = queue.Dequeue();
            if (current == null || !visited.Add(current))
            {
                continue;
            }

            result.Add(current);

            foreach (Neuron connected in current.ConnectedNeurons)
            {
                if (connected != null && !visited.Contains(connected))
                {
                    queue.Enqueue(connected);
                }
            }
        }

        return result;
    }

}
