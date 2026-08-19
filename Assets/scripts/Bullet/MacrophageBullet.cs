using UnityEngine;

public class MacrophageBullet : MonoBehaviour
{
    [SerializeField] private float speed = 15f;
    [SerializeField] float destroyDistance = 15f;
    private Vector2 direction = Vector2.left;

    public void SetDirection(Vector2 newDirection)
    {
        if (newDirection.sqrMagnitude > 0f)
        {
            direction = newDirection.normalized;
        }
    }

    void Update()
    {
        transform.position += (Vector3)(direction * speed * Time.deltaTime);

        // Destroy bullets when they become invisible
        if (transform.position.x < Camera.main.transform.position.x - destroyDistance)
        {
            Destroy(gameObject);
        }
    }
    /* //!なぜか消えない
    // Destroy the bullet when it goes off-screen to prevent memory leaks
    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
    */

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
        else if (other.CompareTag("Bullet"))  // Check if the bullet collides with another bullet
        {
            Destroy(other.gameObject); // Destroy the bullet
            Destroy(gameObject); // Destroy this bullet as well
        }
    }
}
