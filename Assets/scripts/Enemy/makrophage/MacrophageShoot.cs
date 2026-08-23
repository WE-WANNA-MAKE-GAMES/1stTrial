using UnityEngine;

public class MacrophageShoot : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private Transform player;

    [SerializeField] private float fireInterval = 0.2f;

    private void Awake()
    {
        if (player == null)
        {
            GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
            if (playerObject != null)
            {
                player = playerObject.transform;
            }
        }
    }

/*Macrophageには必要ない？
    private PlayerControls controls;

    private void Update()
    {
        // 画面外の敵は攻撃しない
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
            firePoint.position,
            Quaternion.identity
        );
    }

    private bool IsOnScreen()
    {
        Vector3 position = Camera.main.WorldToViewportPoint(transform.position);

            GameObject bulletObject = Instantiate(
                bulletPrefab,
                firePoint.position,
                Quaternion.identity
            );

            if (player != null)
            {
                Vector2 direction =
                    (player.position - firePoint.position).normalized;
                bulletObject.GetComponent<MacrophageBullet>().SetDirection(direction);
            }
        }
    }
}