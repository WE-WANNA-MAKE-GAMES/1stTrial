using UnityEngine;

public class BossNetAttack : MonoBehaviour
{
    [SerializeField] private GameObject netPrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float fireInterval = 4f;
    [SerializeField] private float netScale = 3f;

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

        GameObject net = Instantiate(
            netPrefab,
            firePoint != null ? firePoint.position : transform.position,
            Quaternion.identity
        );

        net.transform.localScale *= netScale;
    }

    private bool IsOnScreen()
    {
        Vector3 position = Camera.main.WorldToViewportPoint(transform.position);

        return position.z > 0f &&
            position.x >= 0f && position.x <= 1f &&
            position.y >= 0f && position.y <= 1f;
    }
}