using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    public float speed = 15f;
    public float damage = 1f;

    [SerializeField] private float destroyDistance = 15f;
    [SerializeField] private float lifeTime = 2f;

    private int remainingPenetrations;

    private readonly HashSet<EntityId> hitEnemyIds = new();

    public void Initialize(int additionalPenetrations)
    {
        remainingPenetrations =
            Mathf.Max(0, additionalPenetrations);
    }

    private void Start()
    {
        PlayerAttack playerAttack =
            FindAnyObjectByType<PlayerAttack>();

        if (playerAttack != null)
        {
            damage = playerAttack.AttackPower;
        }

        Destroy(gameObject, lifeTime);
    }

    private void Update()
    {
        transform.position +=
            transform.right * speed * Time.deltaTime;

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
                other.GetComponentInParent<EnemyHealth>();

            if (enemyHealth == null)
            {
                return;
            }

            EntityId enemyId =
                enemyHealth.GetEntityId();

            // 同じ敵には一度しか当たらない
            if (!hitEnemyIds.Add(enemyId))
            {
                return;
            }

            enemyHealth.TakeDamage(damage);

            // 貫通回数が残っているなら弾は消さない
            if (remainingPenetrations > 0)
            {
                remainingPenetrations--;
                return;
            }

            Destroy(gameObject);
        }
        else if (
            other.CompareTag("Bullet") &&
            other.GetComponent<PlayerBullet>() == null)
        {
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
    }
}