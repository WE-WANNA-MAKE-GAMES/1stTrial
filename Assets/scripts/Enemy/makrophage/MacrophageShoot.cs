using UnityEngine;

public class MacrophageShoot : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firePoint;

    [SerializeField] private float fireInterval = 0.2f;
    private float timer = 0f;

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

            Instantiate(
                bulletPrefab,
                firePoint.position,
                Quaternion.identity
            );
        }
    }
}