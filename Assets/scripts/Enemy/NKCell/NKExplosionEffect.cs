using UnityEngine;

public class NKExplosionEffect : MonoBehaviour
{
    [SerializeField] private GameObject explosionPrefab;

    public void PlayExplosion()
    {
        if (explosionPrefab == null)
        {
            Debug.LogWarning(
                "NKExplosionEffect: Explosion Prefab is not assigned."
            );
            return;
        }

        GameObject explosionObject = Instantiate(
            explosionPrefab,
            transform.position,
            Quaternion.identity,
            transform.parent
        );

        ParticleSystem particleSystem =
            explosionObject.GetComponent<ParticleSystem>();

        if (particleSystem != null)
        {
            particleSystem.Play();
        }
    }
}
//