using UnityEngine;

public class NeutrophilShootNETs : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firePoint;

    [SerializeField] private float fireInterval = 0.2f;
    private float timer = 0f;
    private Transform scrollRoot;

    private void Awake()
    {
        GameObject scrollRootObject = GameObject.FindGameObjectWithTag("ScrollRoot");
        if (scrollRootObject != null)
        {
            scrollRoot = scrollRootObject.transform;
        }
    }

    private void Update()
    {
        timer += Time.deltaTime;

        if (timer >= fireInterval)
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