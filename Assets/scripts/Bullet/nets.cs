using UnityEngine;

public class Nets : MonoBehaviour
{
    [SerializeField] private float speed = -15f;
    [SerializeField] private float disabledTime = 1f;
    [SerializeField] private float destroyDistance = 15f;
    [SerializeField] private GameObject hitEffectPrefab; // 追加：被弾エフェクトのプレハブ

    void Update()
    {
        transform.localPosition += Vector3.right * speed * Time.deltaTime;

        if (transform.position.x < Camera.main.transform.position.x - destroyDistance)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("PlayerDamageReceiver") &&
            !other.CompareTag("Player"))
        {
            return;
        }

        PlayerMovements playerMovements =
            other.GetComponentInParent<PlayerMovements>();

        PlayerInvincibleEffect playerEffect =
            other.GetComponentInParent<PlayerInvincibleEffect>();

        if (playerMovements != null)
        {
            playerMovements.SetDisabled(disabledTime);
        }

        if (playerEffect != null)
        {
            playerEffect.PlayDisabledEffect(disabledTime);
        }

        // 追加：被弾位置にエフェクトを再生
        if (hitEffectPrefab != null)
        {
            Instantiate(
                hitEffectPrefab,
                transform.position,
                Quaternion.identity
            );
        }

        Destroy(gameObject);
    }
}