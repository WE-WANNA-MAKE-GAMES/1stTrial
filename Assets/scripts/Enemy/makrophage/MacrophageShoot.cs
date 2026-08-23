using UnityEngine;

public class MacrophageShoot : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private Transform player;

    [SerializeField] private float fireInterval = 0.2f;
    private float timer = 0f;

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

    private void Awake()
    {
        controls = new PlayerControls();
    }

    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }
*/
    private void Update()
    {
        timer += Time.deltaTime;

        if (/*controls.Player.Shoot.IsPressed() && これもMacrophageに必要なし*/
            timer >= fireInterval)
        {
            timer = 0f;

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