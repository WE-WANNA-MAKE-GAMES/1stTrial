using UnityEngine;
using Manager;

public class PlayerShoot : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firePoint;

    [SerializeField] private float fireInterval = 0.2f;
    private Transform scrollRoot;
    private float timer = 0f;

    private PlayerControls controls;
    private PlayerPiercing playerPiercing;
    private PlayerSpreadShot playerSpreadShot;

    private void Start()
    {
        // If the GameManager is in debug mode, set the fire interval to the debug value
        if (GameManager.Instance != null && GameManager.Instance.IsDebugMode())
        {
            fireInterval = GameManager.Instance.DebugModeShootInterval;
        }
    }

    private void Awake()
    {
        controls = new PlayerControls();
        GameObject scrollRootObject = GameObject.FindGameObjectWithTag("ScrollRoot");
        if (scrollRootObject != null)
        {
            scrollRoot = scrollRootObject.transform;
        }

        playerPiercing = GetComponent<PlayerPiercing>();
        playerSpreadShot = GetComponent<PlayerSpreadShot>();
    }

   private void OnEnable()
{
    if (controls == null)
    {
        controls = new PlayerControls();
    }

    controls.Enable();
}

private void OnDisable()
{
    controls?.Disable();
}

    private void OnDestroy()
    {
        controls.Dispose();
    }

    private void Update()
    {
        if (controls == null) return;
        timer += Time.deltaTime;

        if (controls.Player.Shoot.IsPressed() &&
            timer >= fireInterval)
        {
            timer = 0f;

  if (playerSpreadShot != null && playerSpreadShot.IsActive)
{
    ShootAtAngles(-15f, 0f, 15f);
}
else
{
    ShootAtAngles(0f);
}
    }
}
private void ShootAtAngles(params float[] angles)
{
    foreach (float angle in angles)
    {
        Quaternion rotation = Quaternion.Euler(0f, 0f, angle);

        GameObject bulletObject = Instantiate(
            bulletPrefab,
            firePoint.position,
            rotation,
            scrollRoot
        );

        PlayerBullet bullet = bulletObject.GetComponent<PlayerBullet>();

        if (bullet != null)
        {
            int penetration = playerPiercing != null
                ? playerPiercing.AdditionalPenetrations
                : 0;

            bullet.Initialize(penetration);
        }
    }
}
}

