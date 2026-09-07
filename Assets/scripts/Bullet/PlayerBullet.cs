using UnityEngine;
using Manager;

public class PlayerBullet : MonoBehaviour
{
    public float speed = 15f;   // Speed at which the bullet moves
    public float damage = 1f;   // Damage dealt by the bullet
    [SerializeField] float destroyDistance = 15f;

    private void Start()
    {
        // If the GameManager is in debug mode, set the bullet's damage to the debug value
        if (GameManager.Instance != null && GameManager.Instance.IsDebugMode())
        {
            damage = GameManager.Instance.DebugModeAttackPower;
        }
    }
    void Update()
    {
        transform.localPosition += Vector3.right * speed * Time.deltaTime;

        if (transform.position.x < Camera.main.transform.position.x - destroyDistance)
        {
            Destroy(gameObject);
        }
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
                enemyHealth.TakeDamage(damage); // Deal damage to the enemy
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
