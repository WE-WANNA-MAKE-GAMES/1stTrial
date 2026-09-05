using System.Collections;
using UnityEngine;

public class SelfDestruction : MonoBehaviour
{
    [SerializeField] private float selfDestructionTime = 3f;
    [SerializeField] private float explosionRange = 1.5f;
    [SerializeField] private int damage = 3;

    private Transform player;

    private NKFlash nkFlash;
    private NKExplosionEffect nkExplosionEffect;

    private bool hasExploded = false;

    private void Awake()
    {
        GameObject playerObject =
            GameObject.FindGameObjectWithTag("Player");

        if (playerObject != null)
        {
            player = playerObject.transform;
        }

        nkFlash = GetComponent<NKFlash>();
        nkExplosionEffect = GetComponent<NKExplosionEffect>();
    }

    private void Start()
    {
        if (nkFlash != null)
        {
            nkFlash.StartFlash(selfDestructionTime);
        }

        StartCoroutine(SelfDestruct());
    }

    private IEnumerator SelfDestruct()
    {
        yield return new WaitForSeconds(selfDestructionTime);

        Explode();
    }

    private void Explode()
    {
        if (hasExploded)
        {
            return;
        }

        hasExploded = true;

        Debug.Log($"NK爆発。Playerとの距離: {Vector2.Distance(transform.position, player.position)}");

        if (player != null)
        {
            float distance =
                Vector2.Distance(transform.position, player.position);

            if (distance < explosionRange)
            {
                Debug.Log($"Playerが爆発範囲内にいます。距離: {distance}");
                PlayerHealth playerHealth =
                    player.GetComponentInParent<PlayerHealth>();

                if (playerHealth != null)
                {
                    playerHealth.TakeDamage(
                        damage,
                        transform
                    );
                }
                else
                {
                    Debug.LogWarning("PlayerHealthコンポーネントが見つかりません。");
                }
            }
            else
            {
                Debug.Log($"Playerは爆発範囲外です。距離: {distance}");
            }
        }

        if (nkExplosionEffect != null)
        {
            nkExplosionEffect.PlayExplosion();
        }

        Destroy(gameObject);
    }
}