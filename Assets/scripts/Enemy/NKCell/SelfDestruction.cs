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

        if (player != null)
        {
            float distance =
                Vector2.Distance(transform.position, player.position);

            if (distance < explosionRange)
            {
                PlayerHealth playerHealth =
                    player.GetComponent<PlayerHealth>();

                if (playerHealth != null)
                {
                    playerHealth.TakeDamage(
                        damage,
                        transform
                    );
                }
            }
        }

        if (nkExplosionEffect != null)
        {
            nkExplosionEffect.PlayExplosion();
        }

        Destroy(gameObject);
    }
}