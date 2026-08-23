using UnityEngine;

public class MacrophageBullet : MonoBehaviour
{
    [SerializeField] private float speed = 15f;
    [SerializeField] private float destroyDistance = 15f;

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
        transform.localPosition +=
            (Vector3)(direction * speed * Time.deltaTime);

            // Debug.Log($"Direction: {direction}, speed: {speed}");

        if (Mathf.Abs(transform.position.x - Camera.main.transform.position.x) > destroyDistance)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("PlayerDamageReceiver") ||
            other.CompareTag("Player"))
        {
            PlayerHealth playerHealth =
                other.GetComponentInParent<PlayerHealth>();

            if (playerHealth != null)
            {
                playerHealth.TakeDamage(2, transform);
            }

            Destroy(gameObject);
        }
        else if (other.CompareTag("Bullet"))
        {
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
    }
}