using UnityEngine;

public class BossShoot : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float fireInterval = 2f;

    private float timer;

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
            return;

        timer = 0f;

        Instantiate(
            bulletPrefab,
            firePoint != null ? firePoint.position : transform.position,
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