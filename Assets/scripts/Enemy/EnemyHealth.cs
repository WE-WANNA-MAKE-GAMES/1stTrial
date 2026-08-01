using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField]
    private int maxHP = 3;  // Maximum health points for the enemy
    private int currentHP;  // Current health points of the enemy
    private EnemyEffect enemyEffect; // Reference to the EnemyEffect script for visual feedback
    void Start()
    {
        currentHP = maxHP;  // Initialize current health to maximum health at the start
    }

    private void Awake()
    {
        enemyEffect = GetComponent<EnemyEffect>(); // Get the EnemyEffect component attached to the enemy
    }
    public void TakeDamage(int damage)
    {
        currentHP -= damage;
        enemyEffect.PlayDamageFlash();  // Trigger the damage flash effect when the enemy takes damage
        Debug.Log("Enemy took damage. Current HP: " + currentHP);   // Debug log to check the current HP after taking damage. Should be deleted at launch.
        if (currentHP <= 0)
        {
            enemyEffect.PlayExplosion();  // Trigger the explosion effect when the enemy is destroyed
            Destroy(gameObject);
        }
    }
}
