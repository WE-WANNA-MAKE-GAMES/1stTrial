using UnityEngine;
public class BCellShoot : MonoBehaviour
{
    [SerializeField] private GameObject antibodyPrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float fireInterval = 2f;
    [SerializeField] private float spreadAngle = 40f;
    [SerializeField] private float initialFireDelay = 1f;

    private Transform player;
    private float timer;

    private void Awake()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");

        if (playerObject != null)
        {
            player = playerObject.transform;
        }

        timer = initialFireDelay;
    }

    private void Update()
    {
        if (antibodyPrefab == null || firePoint == null || player == null)
        {
            return;
        }

        timer += Time.deltaTime;

        if (timer >= fireInterval)
        {
            timer = 0f;
            FireFive();
        }
    }

    private void FireFive()
    {
        Vector2 toPlayer =
            ((Vector2)player.position - (Vector2)firePoint.position).normalized;

        float baseAngle =
            Mathf.Atan2(toPlayer.y, toPlayer.x) * Mathf.Rad2Deg;

        // 5発を spreadAngle の範囲に等間隔で並べる
        for (int i = 0; i < 5; i++)
        {
            float offset = Mathf.Lerp(
                -spreadAngle / 2f,
                spreadAngle / 2f,
                i / 4f
            );

            float angle = baseAngle + offset;
            Vector2 direction = new Vector2(
                Mathf.Cos(angle * Mathf.Deg2Rad),
                Mathf.Sin(angle * Mathf.Deg2Rad)
            );

            GameObject bullet = Instantiate(
                antibodyPrefab,
                firePoint.position,
                Quaternion.identity
            );

            HomingAntibody homing = bullet.GetComponent<HomingAntibody>();

            if (homing != null)
            {
                homing.SetTarget(player);
                homing.SetDirection(direction);
            }
        }
    }
}