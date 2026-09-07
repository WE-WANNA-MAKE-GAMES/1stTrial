using UnityEngine;

public class BossShoot : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float fireInterval = 2f;

    [Header("Shotgun")]
    [SerializeField] private int bulletCount = 7;
    [SerializeField] private float spreadAngle = 60f;

    private float timer;
    private Transform player;

    private void Awake()
    {
        GameObject playerObject =
            GameObject.FindGameObjectWithTag("Player");

        if (playerObject != null)
        {
            player = playerObject.transform;
        }
    }

    private void Update()
    {
        // Bossが画面外なら攻撃しない
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
        if (bulletPrefab == null)
        {
            return;
        }

        Vector3 spawnPosition =
            firePoint != null
                ? firePoint.position
                : transform.position;

        // プレイヤーがいない場合は右方向
        Vector2 centerDirection = Vector2.left;

        if (player != null)
        {
            centerDirection =
                (player.position - spawnPosition).normalized;
        }

        float startAngle = -spreadAngle / 2f;

        float angleStep =
            bulletCount > 1
                ? spreadAngle / (bulletCount - 1)
                : 0f;

        for (int i = 0; i < bulletCount; i++)
        {
            float angle =
                startAngle + angleStep * i;

            Vector2 direction =
                RotateVector(centerDirection, angle);

            GameObject bullet =
                Instantiate(
                    bulletPrefab,
                    spawnPosition,
                    Quaternion.identity
                );

            Stage2BossBullet bossBullet =
                bullet.GetComponent<Stage2BossBullet>();

            if (bossBullet != null)
            {
                bossBullet.SetDirection(direction);
            }
        }
    }

    private Vector2 RotateVector(
        Vector2 vector,
        float angle
    )
    {
        float radians =
            angle * Mathf.Deg2Rad;

        float cos =
            Mathf.Cos(radians);

        float sin =
            Mathf.Sin(radians);

        return new Vector2(
            vector.x * cos - vector.y * sin,
            vector.x * sin + vector.y * cos
        ).normalized;
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

        return position.z > 0f &&
            position.x >= 0f &&
            position.x <= 1f &&
            position.y >= 0f &&
            position.y <= 1f;
    }
}