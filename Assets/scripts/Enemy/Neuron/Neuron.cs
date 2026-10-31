using System.Collections.Generic;
using UnityEngine;

public class Neuron : MonoBehaviour
{
    [SerializeField]
    private List<Neuron> connectedNeurons = new();

    [SerializeField]
    private bool autoConnectToNearbyNeurons = false;

    [SerializeField]
    private float nearbyConnectionDistance = 6f;

    [SerializeField]
    private float contactRadius = 0.5f;

    public IReadOnlyList<Neuron> ConnectedNeurons => connectedNeurons;

    private void Reset()
    {
        if (GetComponent<NeuronConnection>() == null)
        {
            gameObject.AddComponent<NeuronConnection>();
        }
        if (GetComponent<NeuronElectricAttack>() == null)
        {
            gameObject.AddComponent<NeuronElectricAttack>();
        }
    }

    private void Awake()
    {
        RemoveDestroyedConnections();

        if (GetComponent<Collider2D>() == null)
        {
            CircleCollider2D collider = gameObject.AddComponent<CircleCollider2D>();
            float scale = Mathf.Max(
                Mathf.Abs(transform.lossyScale.x),
                Mathf.Abs(transform.lossyScale.y)
            );
            collider.radius = contactRadius / Mathf.Max(scale, 0.0001f);
            collider.isTrigger = true;
        }

        if (GetComponent<NeuronConnection>() == null)
        {
            gameObject.AddComponent<NeuronConnection>();
        }

        if (GetComponent<NeuronElectricAttack>() == null)
        {
            gameObject.AddComponent<NeuronElectricAttack>();
        }
    }

    private void Update()
    {
        RemoveDestroyedConnections();

        if (autoConnectToNearbyNeurons)
        {
            AutoConnectNearbyNeurons();
        }
    }

    public void ConnectTo(Neuron other)
    {
        if (other == null || other == this)
        {
            return;
        }

        if (connectedNeurons.Contains(other))
        {
            return;
        }

        connectedNeurons.Add(other);
        other.AddConnectionFrom(this);
    }

    public void AddConnectionFrom(Neuron other)
    {
        if (other == null || other == this)
        {
            return;
        }

        if (connectedNeurons.Contains(other))
        {
            return;
        }

        connectedNeurons.Add(other);
    }

    public void RemoveConnection(Neuron other)
    {
        if (other == null)
        {
            return;
        }

        connectedNeurons.RemoveAll(neuron => neuron == null || neuron == other);
    }

    public void RemoveDestroyedConnections()
    {
        connectedNeurons.RemoveAll(neuron => neuron == null || neuron == this);
    }

    private void AutoConnectNearbyNeurons()
    {
        Neuron[] allNeurons = FindObjectsByType<Neuron>();

        foreach (Neuron other in allNeurons)
        {
            if (other == null || other == this)
            {
                continue;
            }

            float distance = Vector2.Distance(
                transform.position,
                other.transform.position
            );

            if (distance <= nearbyConnectionDistance)
            {
                ConnectTo(other);
            }
        }
    }
}
