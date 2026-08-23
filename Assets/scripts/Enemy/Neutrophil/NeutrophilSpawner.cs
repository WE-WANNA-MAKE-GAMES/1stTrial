using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab; // Reference to the enemy prefab to spawn
    [SerializeField] private float spawnInterval = 1f;   // Time interval between enemy spawns in seconds
    private float timer = 0f;    // Timer to track time since last spawn
    [SerializeField] private float minY = -4f;   // Minimum Y position for enemy spawn
    [SerializeField] private float maxY = 4f;    // Maximum Y position for enemy spawn
    [SerializeField] private Transform player; // Reference to the player transform to determine spawn position
    [SerializeField] private float spawnOffsetX = 5f; // Distance from the player at which enemies will spawn
    [SerializeField] private Transform scrollRoot;

    private void Update()
    {
        timer += Time.deltaTime;    // Increment the timer by the time elapsed since the last frame

        if (timer >= spawnInterval)
        {
            timer = 0f;

            SpawnEnemy();
        }
    }

    private void SpawnEnemy()
    {
        Camera camera = Camera.main;

        Vector3 rightEdge =
            camera.ViewportToWorldPoint(
                new Vector3(1, 0.5f, camera.nearClipPlane)
            );

        float randomY =
            Random.Range(minY, maxY);

        Vector3 spawnPosition =
            new Vector3(
                rightEdge.x + spawnOffsetX,
                randomY,
                player.position.z
            );

        Instantiate(
            enemyPrefab,
            spawnPosition,
            Quaternion.identity,
            scrollRoot   // 追加
        );
    }
}