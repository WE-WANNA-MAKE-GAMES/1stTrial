using UnityEngine;

public class MacrophageShoot : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private Transform player;

    [SerializeField] private float fireInterval = 0.2f;
    [SerializeField] private float initialFireDelayMin = 0f;
    [SerializeField] private float initialFireDelayMax = 2f;
    private Transform scrollRoot;
    private float timer = 0f;

    private void Awake()
    {
        timer = Random.Range(initialFireDelayMin, initialFireDelayMax);

        if (player == null)
        {
            GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
            if (playerObject != null)
            {
                player = playerObject.transform;
            }
        };
        GameObject scrollRootObject = GameObject.FindGameObjectWithTag("ScrollRoot");
        if (scrollRootObject != null)
        {
            scrollRoot = scrollRootObject.transform;
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
            timer >= GetFireInterval())
        {
            timer = 0f;

            GameObject bulletObject = Instantiate(
                bulletPrefab,
                firePoint.position,
                Quaternion.identity,
                scrollRoot
            );

            if (player != null)
            {
                Vector2 direction =
                    (player.position - firePoint.position).normalized;
                bulletObject.GetComponent<MacrophageBullet>().SetDirection(direction);
            }
        }
    }

    private float GetFireInterval()
    {
        EnemyBuffReceiver receiver = GetComponent<EnemyBuffReceiver>();
        return fireInterval / (receiver != null ? receiver.SpeedMultiplier : 1f);
    }
}