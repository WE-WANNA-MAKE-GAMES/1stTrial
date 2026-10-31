using System.Collections.Generic;
using UnityEngine;

public class Stage6Boss : MonoBehaviour
{
    [SerializeField]
    private Transform scrollRoot;

    [SerializeField]
    private List<Neuron> connectedNeurons = new();

    private Neuron rootNeuron;
    private Stage6BossSpawner spawner;

    private void Awake()
    {
        gameObject.tag = "Enemy";

        if (GetComponent<EnemyHealth>() == null)
        {
            gameObject.AddComponent<EnemyHealth>();
        }

        if (GetComponent<EnemyEffect>() == null)
        {
            gameObject.AddComponent<EnemyEffect>();
        }

        if (GetComponent<EnemyKnockback>() == null)
        {
            gameObject.AddComponent<EnemyKnockback>();
        }

        if (GetComponent<Rigidbody2D>() == null)
        {
            Rigidbody2D rigidbody2D = gameObject.AddComponent<Rigidbody2D>();
            rigidbody2D.gravityScale = 0f;
        }

        if (GetComponent<Collider2D>() == null)
        {
            BoxCollider2D boxCollider = gameObject.AddComponent<BoxCollider2D>();
            boxCollider.isTrigger = true;
        }

        if (GetComponent<Neuron>() == null)
        {
            rootNeuron = gameObject.AddComponent<Neuron>();
        }
        else
        {
            rootNeuron = GetComponent<Neuron>();
        }

        if (GetComponent<Stage6BossSpawner>() == null)
        {
            spawner = gameObject.AddComponent<Stage6BossSpawner>();
        }
        else
        {
            spawner = GetComponent<Stage6BossSpawner>();
        }

        if (scrollRoot == null)
        {
            GameObject scrollRootObject = GameObject.FindGameObjectWithTag("ScrollRoot");
            if (scrollRootObject != null)
            {
                scrollRoot = scrollRootObject.transform;
            }
        }
    }

    private void Start()
    {
        if (rootNeuron != null)
        {
            foreach (Neuron neuron in connectedNeurons)
            {
                if (neuron != null)
                {
                    rootNeuron.ConnectTo(neuron);
                }
            }
        }
    }

    private void Update()
    {
        if (spawner != null)
        {
            spawner.SetScrollRoot(scrollRoot);
        }
    }

    public void RegisterNeuron(Neuron neuronToConnect)
    {
        if (neuronToConnect == null || rootNeuron == null)
        {
            return;
        }

        rootNeuron.ConnectTo(neuronToConnect);
    }

    public void StopSpawning()
    {
        if (spawner != null)
        {
            spawner.StopSpawning();
        }
    }

    private void OnDestroy()
    {
        StopSpawning();
    }
}
