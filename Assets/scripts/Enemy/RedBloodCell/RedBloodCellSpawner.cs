using UnityEngine;

public class RedBloodCellSpawner : MonoBehaviour
{
    [SerializeField] private GameObject redBloodCellPrefab; // Reference to the macrophage prefab to spawn
    [SerializeField] private float spawnInterval = 1f;   // Time interval between macrophage spawns in seconds
    private float timer = 0f;    // Timer to track time since last spawn
    [SerializeField] private float minY = -4f;   // Minimum Y position for macrophage spawn
    [SerializeField] private float maxY = 4f;    // Maximum Y position for macrophage spawn
    [SerializeField] private Transform player; // Reference to the player transform to determine spawn position
    [SerializeField] private float spawnOffsetX = 5f; // Distance from the player at which macrophages will spawn

    private void Update()
    {
        timer += Time.deltaTime;    // Increment the timer by the time elapsed since the last frame

        if (timer >= spawnInterval)
        {
            timer = 0f;

            SpawnRedBloodCell();
        }
    }

    private void SpawnRedBloodCell()
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
            redBloodCellPrefab,
            spawnPosition,
            Quaternion.identity
        );
    }
}