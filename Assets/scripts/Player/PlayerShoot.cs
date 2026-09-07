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
    }

    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }

    private void Update()
    {
        timer += Time.deltaTime;

        if (controls.Player.Shoot.IsPressed() &&
            timer >= fireInterval)
        {
            timer = 0f;

            Instantiate(
                bulletPrefab,
                firePoint.position,
                Quaternion.identity,
                scrollRoot
            );
        }
    }
}