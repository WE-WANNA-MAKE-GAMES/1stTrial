using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    public float speed = 15f;
    public float damage = 1f;

    [SerializeField] private float destroyDistance = 15f;

    private void Start()
    {
        PlayerAttack playerAttack =
            FindAnyObjectByType<PlayerAttack>();

        if (playerAttack != null)
        {
            damage = playerAttack.AttackPower;
        }
    }

    private void Update()
    {
        transform.localPosition +=
            Vector3.right * speed * Time.deltaTime;

        if (Camera.main != null &&
            transform.position.x <
            Camera.main.transform.position.x - destroyDistance)
        {
            Destroy(gameObject);
        }
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            EnemyHealth enemyHealth =
                other.GetComponent<EnemyHealth>();

            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(damage);
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