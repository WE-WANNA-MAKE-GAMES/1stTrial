using UnityEngine;

public class KupfferShoot : MonoBehaviour
{
    [SerializeField] private GameObject detoxPrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float fireInterval = 3f;

    private float timer;

    private Transform player;
    private Transform scrollRoot;

    private void Awake()
    {
        GameObject playerObject =
            GameObject.FindGameObjectWithTag("Player");

        if (playerObject != null)
        {
            player = playerObject.transform;
        }

        GameObject scrollRootObject =
            GameObject.FindGameObjectWithTag("ScrollRoot");

        if (scrollRootObject != null)
        {
            scrollRoot = scrollRootObject.transform;
        }
    }

    private void Update()
    {
        // クッパー細胞が画面外なら攻撃しない
        if (!IsOnScreen())
        {
            timer = 0f;
            return;
        }

        timer += Time.deltaTime;

        if (timer < fireInterval)
        {
            return;
        }

        timer = 0f;

        Shoot();
    }

    private void Shoot()
    {
        if (detoxPrefab == null)
        {
            return;
        }

        Vector3 spawnPosition =
            firePoint != null
                ? firePoint.position
                : transform.position;

        Vector2 direction = Vector2.left;

        if (player != null)
        {
            direction =
                (player.position - spawnPosition).normalized;
        }

        GameObject detox;

        if (scrollRoot != null)
        {
            detox = Instantiate(
                detoxPrefab,
                spawnPosition,
                Quaternion.identity,
                scrollRoot
            );
        }
        else
        {
            detox = Instantiate(
                detoxPrefab,
                spawnPosition,
                Quaternion.identity
            );
        }

        Detox detoxScript =
            detox.GetComponent<Detox>();

        if (detoxScript != null)
        {
            detoxScript.SetDirection(direction);
        }
        else
        {
            Debug.LogWarning(
                "KupfferShoot: Detox PrefabにDetoxがありません。"
            );
        }
    }

    private bool IsOnScreen()
    {
        if (Camera.main == null)
        {
            return false;
        }

        Vector3 position =
            Camera.main.WorldToViewportPoint(
                transform.position
            );

        return
            position.z > 0f &&
            position.x >= 0f &&
            position.x <= 1f &&
            position.y >= 0f &&
            position.y <= 1f;
    }
}