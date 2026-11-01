using UnityEngine;

public class Stage6BossSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject microgliaPrefab;

    [SerializeField]
    private float spawnInterval = 5f;

    [SerializeField]
    private Vector2 spawnOffset = new(-1.5f, 0f);

    [SerializeField]
    private float spawnVarianceY = 1.5f;

    private float timer;
    private Transform scrollRoot;
    private bool spawningEnabled = true;

    private void Awake()
    {
        if (scrollRoot == null)
        {
            GameObject scrollRootObject = GameObject.FindGameObjectWithTag("ScrollRoot");
            if (scrollRootObject != null)
            {
                scrollRoot = scrollRootObject.transform;
            }
        }
    }

    private void Update()
    {
        if (!spawningEnabled || microgliaPrefab == null)
        {
            return;
        }

        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            timer = 0f;
            SpawnMicroglia();
        }
    }

    public void SetScrollRoot(Transform root)
    {
        scrollRoot = root;
    }

    public void StopSpawning()
    {
        spawningEnabled = false;
    }

    public void ResumeSpawning()
    {
        spawningEnabled = true;
    }

    private void SpawnMicroglia()
    {
        if (microgliaPrefab == null)
        {
            return;
        }

        Vector3 spawnPosition = transform.position + new Vector3(spawnOffset.x, Random.Range(-spawnVarianceY, spawnVarianceY), 0f);

        Instantiate(
            microgliaPrefab,
            spawnPosition,
            Quaternion.identity,
            scrollRoot
        );
    }
}
