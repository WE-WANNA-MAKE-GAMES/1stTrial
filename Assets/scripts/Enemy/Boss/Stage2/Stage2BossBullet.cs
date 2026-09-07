using UnityEngine;

public class Stage2BossBullet : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private float damage = 1f;
    [SerializeField] private float destroyDistance = 15f;

    private Vector2 direction = Vector2.left;

    public void SetDirection(Vector2 direction)
    {
        this.direction = direction.normalized;
    }

    private void Update()
    {
        transform.position +=
            (Vector3)(direction * speed * Time.deltaTime);

        CheckDestroyDistance();
    }

    private void CheckDestroyDistance()
    {
        if (Camera.main == null)
        {
            return;
        }

        float distance =
            Vector2.Distance(
                transform.position,
                Camera.main.transform.position
            );

        if (distance > destroyDistance)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player") &&
            !other.CompareTag("PlayerDamageReceiver"))
        {
            return;
        }

        PlayerHealth playerHealth =
            other.GetComponentInParent<PlayerHealth>();

        if (playerHealth != null)
        {
            playerHealth.TakeDamage(
                damage,
                transform
            );
        }

        Destroy(gameObject);
    }
}