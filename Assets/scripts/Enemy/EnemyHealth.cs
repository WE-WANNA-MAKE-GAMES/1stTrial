using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField]
    private int maxHP = 3;  // Maximum health points for the enemy
    private int currentHP;  // Current health points of the enemy
    private EnemyEffect enemyEffect; // Reference to the EnemyEffect script for visual feedback
    private EnemyKnockback knockback; // Reference to the EnemyKnockback script for knockback effect
    void Start()
    {
        currentHP = maxHP;  // Initialize current health to maximum health at the start
    }

    private void Awake()
    {
        enemyEffect = GetComponent<EnemyEffect>(); // Get the EnemyEffect component attached to the enemy
        knockback = GetComponent<EnemyKnockback>(); // Get the EnemyKnockback component attached to the enemy
    }
    public void TakeDamage(int damage)
    {
        currentHP -= damage;    // Reduce current health by the damage amount
        knockback.Knockback(Vector2.right);  // Apply knockback effect to the
        enemyEffect.PlayDamageFlash();  // Trigger the damage flash effect when the enemy takes damage
        Debug.Log("Enemy took damage. Current HP: " + currentHP);   // Debug log to check the current HP after taking damage. Should be deleted at launch.
        if (currentHP <= 0)
        {
            enemyEffect.PlayExplosion();  // Trigger the explosion effect when the enemy is destroyed
            Destroy(gameObject);
        }
    }
}
