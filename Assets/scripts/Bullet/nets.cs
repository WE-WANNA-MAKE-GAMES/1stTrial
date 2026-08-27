using UnityEngine;

public class Nets : MonoBehaviour
{
    [SerializeField] private float speed = -15f;
    [SerializeField] private float disabledTime = 1f;
    [SerializeField] private float destroyDistance = 15f;
    [SerializeField] private GameObject hitEffectPrefab; // NETs被弾時のエフェクト用プレハブ

    private void Update()
    {
        Move();
        CheckOutOfScreen();
    }

    /// <summary>
    /// NETs弾を移動させる
    /// </summary>
    private void Move()
    {
        transform.localPosition += Vector3.right * speed * Time.deltaTime;
    }

    /// <summary>
    /// 画面外に出た場合に自身を破棄する
    /// </summary>
    private void CheckOutOfScreen()
    {
        if (Camera.main != null && transform.position.x < Camera.main.transform.position.x - destroyDistance)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!IsTargetPlayer(other))
        {
            return;
        }

        ApplyDisabledToPlayer(other);
        SpawnHitEffect();
        Destroy(gameObject);
    }

    /// <summary>
    /// 衝突対象がプレイヤーかどうかを判定する
    /// </summary>
    private bool IsTargetPlayer(Collider2D other)
    {
        return other.CompareTag("PlayerDamageReceiver") || other.CompareTag("Player");
    }

    /// <summary>
    /// プレイヤーに行動不能（スタン）および白化エフェクトを適用する
    /// </summary>
    private void ApplyDisabledToPlayer(Collider2D other)
    {
        PlayerMovements playerMovements = other.GetComponentInParent<PlayerMovements>();
        if (playerMovements != null)
        {
            playerMovements.SetDisabled(disabledTime);
        }

        PlayerDisabledEffect playerDisabledEffect = other.GetComponentInParent<PlayerDisabledEffect>();
        if (playerDisabledEffect != null)
        {
            playerDisabledEffect.PlayDisabledEffect(disabledTime);
        }
    }

    /// <summary>
    /// 被弾位置にエフェクトを生成する
    /// </summary>
    private void SpawnHitEffect()
    {
        if (hitEffectPrefab != null)
        {
            Instantiate(
                hitEffectPrefab,
                transform.position,
                Quaternion.identity
            );
        }
    }
}