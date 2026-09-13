using UnityEngine;

public class Detox : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private float attackPowerMultiplier = 0.5f;
    [SerializeField] private float duration = 10f;

    private Vector2 direction = Vector2.left;

    public void SetDirection(Vector2 direction)
    {
        this.direction = direction.normalized;
    }

    private void Update()
    {
        transform.position +=
            (Vector3)(direction * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Detox hit: " + other.gameObject.name);

        if (!other.CompareTag("Player") &&
            !other.CompareTag("PlayerDamageReceiver"))
        {
            return;
        }

        Debug.Log("Player detected!");

        PlayerAttack playerAttack =
            other.GetComponentInParent<PlayerAttack>();

        if (playerAttack != null)
        {
            float currentAttackPower =
                playerAttack.AttackPower;

            Debug.Log(
                "Current Attack Power: " + currentAttackPower
            );

            playerAttack.ApplyAttackPowerDebuff(
                attackPowerMultiplier,
                duration
            );
        }
        else
        {
            Debug.LogWarning(
                "PlayerAttack が見つかりません"
            );
        }

        Destroy(gameObject);
    }
}