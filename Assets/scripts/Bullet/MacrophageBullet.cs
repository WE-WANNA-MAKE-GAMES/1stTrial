using UnityEngine;

public class MacrophageBullet : MonoBehaviour
{
    public float speed = -15f;   // Speed at which the bullet moves
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
        if (other.CompareTag("PlayerDamageReceiver") || other.CompareTag("Player"))  // Check if the bullet collides with the player
        {
            PlayerHealth playerHealth = other.GetComponentInParent<PlayerHealth>();    // Get the PlayerHealth component from the player or parent object
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(2, transform); // Deal 2 damage to the player
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
