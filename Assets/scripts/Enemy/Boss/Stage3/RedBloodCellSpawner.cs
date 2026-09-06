using UnityEngine;

public class RedBloodCellSpawner : MonoBehaviour
{
    [SerializeField] private GameObject redBloodCellPrefab;
    [SerializeField] private float spawnInterval = 2f;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private float minY = -4f;   // Minimum Y position for enemy spawn
    [SerializeField] private float maxY = 4f;    // Maximum Y position for enemy spawn

    private float timer;

    private void Update()
    {
        // Bossが画面外ならスポーンしない
        if (!IsOnScreen())
        {
            timer = 0f;
            return;
        }

        timer += Time.deltaTime;

        if (timer < spawnInterval)
            return;

        timer = 0f;

        float randomY = Random.Range(minY, maxY);

        spawnPoint.position = new Vector3(spawnPoint.position.x, randomY, spawnPoint.position.z);

        Instantiate(
            redBloodCellPrefab,
            spawnPoint.position,
            Quaternion.identity
        );
    }

    private bool IsOnScreen()
    {
        Vector3 position = Camera.main.WorldToViewportPoint(transform.position);

        return position.z > 0f &&
            position.x >= 0f && position.x <= 1f &&
            position.y >= 0f && position.y <= 1f;
    }
}