using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    public float speed = 15f;
    [SerializeField] private float destroyDistance = 15f;
    [SerializeField] private float lifeTime = 2f;

    private int remainingPenetrations;
    private readonly HashSet<EntityId> hitEnemyIds = new();

    public void Initialize(int additionalPenetrations)
    {
        remainingPenetrations = Mathf.Max(0, additionalPenetrations);
    }

    private void Update()
    {
        transform.position += transform.right * speed * Time.deltaTime;
        if (transform.position.x <
            Camera.main.transform.position.x - destroyDistance)
        {
            Destroy(gameObject);
        }
    }
    private void Start()
{
    Destroy(gameObject, lifeTime);
}

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            // 子Colliderを使う敵にも対応するため Parent を検索する
            EnemyHealth enemyHealth =
                other.GetComponentInParent<EnemyHealth>();

            if (enemyHealth == null) return;

            // 同一の敵への複数ヒットを防ぐ
            EntityId enemyId = enemyHealth.GetEntityId();

            if (!hitEnemyIds.Add(enemyId)) return;

            enemyHealth.TakeDamage(1);

            if (remainingPenetrations > 0)
            {
                remainingPenetrations--;
                return;
            }

            Destroy(gameObject);
        }
       else if (other.CompareTag("Bullet") &&
         other.GetComponent<PlayerBullet>() == null)
{
    Destroy(other.gameObject);
    Destroy(gameObject);
}
    }
}