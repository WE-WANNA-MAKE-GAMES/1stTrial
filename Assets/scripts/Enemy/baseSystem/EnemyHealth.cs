using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField]
    private float maxHP = 1f;

    private float currentHP;

    private EnemyEffect enemyEffect;
    private EnemyKnockback knockback;

    public float MaxHP => maxHP;
    public float CurrentHP => Mathf.Max(currentHP, 0f);

    private void Start()
    {
        currentHP = maxHP;
    }

    private void Awake()
    {
        enemyEffect = GetComponent<EnemyEffect>();
        knockback = GetComponent<EnemyKnockback>();
    }

    public void TakeDamage(float damage)
    {
        currentHP -= damage;

        if (knockback != null &&
            knockback.isActiveAndEnabled)
        {
            knockback.Knockback(Vector2.right);
        }

        if (enemyEffect != null)
        {
            enemyEffect.PlayDamageFlash();
        }

        Debug.Log(
            "Enemy took damage. Current HP: " + currentHP
        );

        if (currentHP <= 0)
        {
            GetComponent<HealthItemDrop>()?.Drop();
            GetComponent<SpeedItemDrop>()?.Drop();
            GetComponent<PiercingItemDrop>()?.Drop();
            GetComponent<SpreadShotItemDrop>()?.Drop();

            if (enemyEffect != null)
            {
                enemyEffect.PlayExplosion();
            }

            Destroy(gameObject);
        }
    }
}