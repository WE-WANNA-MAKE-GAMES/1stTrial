using System.Collections;
using UnityEngine;

public class EnemyEffect : MonoBehaviour
{
    [SerializeField] private GameObject explosionPrefab; // Reference to the explosion effect prefab
    private SpriteRenderer spriteRenderer; // Reference to the SpriteRenderer component
    private Color originalColor; // Original color of the sprite

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;
    }
    public void PlayDamageFlash()
    {
        StartCoroutine(DamageFlash());
    }
    //* 敵がダメージを受けたときのフラッシュエフェクト
    private IEnumerator DamageFlash()
    {
        // Change the sprite color to red
        spriteRenderer.color = Color.red;

        // Wait for a short duration (e.g., 0.1 seconds)
        yield return new WaitForSeconds(0.1f);

        // Revert the sprite color back to the original color
        spriteRenderer.color = originalColor;
    }
    //* 敵の爆発エフェクト
    public void PlayExplosion()
    {
        Instantiate(
            explosionPrefab,
            transform.position,
            Quaternion.identity
        );
    }
}