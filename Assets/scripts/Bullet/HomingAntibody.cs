using UnityEngine;

public class HomingAntibody : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private float turnSpeed = 180f;
    [SerializeField] private float lifeTime = 8f;
    [SerializeField] private int damage = 1;

    private Transform target;
    private Vector2 direction = Vector2.left;

    private void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }

    public void SetDirection(Vector2 newDirection)
    {
        if (newDirection.sqrMagnitude > 0f)
        {
            direction = newDirection.normalized;
        }
    }

    private void Update()
    {
        if (target != null)
        {
            Vector2 toTarget =
                (Vector2)target.position - (Vector2)transform.position;

            if (toTarget.sqrMagnitude > 0f)
            {
                float currentAngle =
                    Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

                float targetAngle =
                    Mathf.Atan2(toTarget.y, toTarget.x) * Mathf.Rad2Deg;

                float newAngle = Mathf.MoveTowardsAngle(
                    currentAngle,
                    targetAngle,
                    turnSpeed * Time.deltaTime
                );

                direction = new Vector2(
                    Mathf.Cos(newAngle * Mathf.Deg2Rad),
                    Mathf.Sin(newAngle * Mathf.Deg2Rad)
                );
            }
        }

        transform.position +=
            (Vector3)(direction * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("PlayerDamageReceiver") &&
            !other.CompareTag("Player"))
        {
            return;
        }

        PlayerHealth health = other.GetComponentInParent<PlayerHealth>();

        if (health != null)
        {
            health.TakeDamage(damage, transform);
        }

        Destroy(gameObject);
    }
}