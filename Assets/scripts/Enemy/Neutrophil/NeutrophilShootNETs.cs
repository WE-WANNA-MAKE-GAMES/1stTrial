using UnityEngine;

public class NeutrophilShootNETs : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firePoint;

    [SerializeField] private float fireInterval = 0.2f;
    [SerializeField] private float initialFireDelayMin = 0f;
    [SerializeField] private float initialFireDelayMax = 2f;
    private float timer = 0f;
    private Transform scrollRoot;

    private void Awake()
    {
        timer = Random.Range(initialFireDelayMin, initialFireDelayMax);

        GameObject scrollRootObject = GameObject.FindGameObjectWithTag("ScrollRoot");
        if (scrollRootObject != null)
        {
            scrollRoot = scrollRootObject.transform;
        }
    }

    private void Update()
    {
        timer += Time.deltaTime;

        if (timer >= GetFireInterval())
        {
            timer = 0f;

            if (bulletPrefab == null || firePoint == null)
            {
                return;
            }

            Instantiate(
                bulletPrefab,
                firePoint.position,
                Quaternion.identity,
                scrollRoot
            );
        }
    }

    private float GetFireInterval()
    {
        EnemyBuffReceiver receiver = GetComponent<EnemyBuffReceiver>();
        return fireInterval / (receiver != null ? receiver.SpeedMultiplier : 1f);
    }
}