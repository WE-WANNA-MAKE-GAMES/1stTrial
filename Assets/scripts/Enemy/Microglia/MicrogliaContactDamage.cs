using UnityEngine;

public class MicrogliaContactDamage : MonoBehaviour
{
    [SerializeField]
    private float contactDamage = 2f;

    [SerializeField]
    private float damageCooldown = 0.5f;

    private float nextDamageAt;

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other == null)
        {
            return;
        }

        if (!other.CompareTag("Player") && !other.CompareTag("PlayerDamageReceiver"))
        {
            return;
        }

        if (Time.time < nextDamageAt)
        {
            return;
        }

        PlayerHealth playerHealth = other.GetComponentInParent<PlayerHealth>();
        if (playerHealth == null)
        {
            return;
        }

        playerHealth.TakeDamage(contactDamage, transform);
        nextDamageAt = Time.time + damageCooldown;
    }
}
