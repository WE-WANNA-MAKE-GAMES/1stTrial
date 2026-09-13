using UnityEngine;

public class KupfferSpawner : MonoBehaviour
{
    [Header("Enemy")]
    [SerializeField]
    private GameObject enemyPrefab;

    [SerializeField]
    private int spawnCount = 1;

    [Header("Spawn Trigger")]
    [SerializeField]
    private float spawnDistance = 15f;

    private bool hasSpawned = false;

    private Camera mainCamera;

    private void Awake()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        if (hasSpawned)
        {
            return;
        }

        if (mainCamera == null)
        {
            mainCamera = Camera.main;

            if (mainCamera == null)
            {
                return;
            }
        }

        float distance =
            transform.position.x - mainCamera.transform.position.x;

        if (distance <= spawnDistance)
        {
            SpawnEnemy();
        }
    }

    private void SpawnEnemy()
    {
        if (enemyPrefab == null)
        {
            Debug.LogError(
                $"EnemySpawner on {gameObject.name} has no enemyPrefab assigned.",
                this
            );

            return;
        }

        hasSpawned = true;

        for (int i = 0; i < spawnCount; i++)
        {
            Instantiate(
                enemyPrefab,
                transform.position,
                Quaternion.identity
            );
        }
    }
}