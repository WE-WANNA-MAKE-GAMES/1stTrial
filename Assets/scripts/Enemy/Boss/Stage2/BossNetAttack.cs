using UnityEngine;

public class BossNetAttack : MonoBehaviour
{
    [SerializeField] private GameObject netPrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float fireInterval = 4f;
    [SerializeField] private float initialFireDelayMin = 0f;
    [SerializeField] private float initialFireDelayMax = 2f;
    [SerializeField] private float netScale = 3f;

    private float timer;
    private bool hasStartedAttacking;

    private void Update()
    {
        // Bossが画面外なら攻撃しない
        if (!IsOnScreen())
        {
            if (!hasStartedAttacking)
            {
                timer = 0f;
            }
            return;
        }

        if (!hasStartedAttacking)
        {
            timer = Random.Range(initialFireDelayMin, initialFireDelayMax);
            hasStartedAttacking = true;
        }

        timer += Time.deltaTime;

        if (timer < GetFireInterval())
            return;

        timer = 0f;

        if (netPrefab == null)
        {
            return;
        }

        GameObject net = Instantiate(
            netPrefab,
            firePoint != null ? firePoint.position : transform.position,
            Quaternion.identity
        );

        net.transform.localScale *= netScale;
    }

    private float GetFireInterval()
    {
        EnemyBuffReceiver receiver = GetComponent<EnemyBuffReceiver>();
        return fireInterval / (receiver != null ? receiver.SpeedMultiplier : 1f);
    }

    private bool IsOnScreen()
    {
        if (Camera.main == null)
        {
            return false;
        }

        Vector3 position = Camera.main.WorldToViewportPoint(transform.position);

        return position.z > 0f &&
            position.x >= 0f && position.x <= 1f &&
            position.y >= 0f && position.y <= 1f;
    }
}