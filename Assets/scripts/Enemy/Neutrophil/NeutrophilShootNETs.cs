using UnityEngine;

public class NeutrophilShootNETs : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firePoint;

    [SerializeField] private float fireInterval = 0.2f;
    private float timer = 0f;

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