using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    public float speed = 15f;   // Speed at which the bullet moves
    void Update()
    {
        transform.position += Vector3.right * speed * Time.deltaTime;   // Move the bullet to the right at the specified speed
    }
    // Destroy the bullet when it goes off-screen to prevent memory leaks
    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))  // Check if the bullet collides with an enemy
        {
            EnemyHealth enemyHealth = other.GetComponent<EnemyHealth>();    // Get the EnemyHealth component from the enemy that was hit
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(1); // Deal 1 damage to the enemy
            }
            Destroy(gameObject);
        }
        else if(other.CompareTag("Bullet"))  // Check if the bullet collides with another bullet
        {
            Destroy(other.gameObject); // Destroy the bullet
            Destroy(gameObject); // Destroy this bullet as well
        }
    }
}
