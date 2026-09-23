using UnityEngine;

public class DetoxItem : MonoBehaviour
{
    [SerializeField] private float attackPowerMultiplier = 0.5f;
    [SerializeField] private float duration = 10f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerAttack playerAttack =
            other.GetComponentInParent<PlayerAttack>();

        if (playerAttack == null)
        {
            return;
        }

        playerAttack.ApplyAttackPowerDebuff(
            attackPowerMultiplier,
            duration
        );

        Destroy(gameObject);
    }
}