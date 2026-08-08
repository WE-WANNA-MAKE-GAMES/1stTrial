using System.Collections;
using UnityEngine;

public class PlayerInvincibleEffect : MonoBehaviour
{
    private SpriteRenderer spriteRenderer; // Reference to the SpriteRenderer component
    private Color originalColor; // Original color of the sprite
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;
    }

    //* プレイヤーが無敵状態のときのフラッシュエフェクトの呼び出し
    public void PlayInvincibleEffect(float duration)
    {
        StartCoroutine(InvincibleFlash(duration));
    }

    //* プレイヤーが無敵状態のときのフラッシュエフェクトの処理
    private IEnumerator InvincibleFlash(float duration)
    {
        float timer = 0f;
        while (timer < duration)
        {
            Color color = originalColor;
            color.a = 0.3f; // Set the alpha value to 0.5 for semi-transparency
            spriteRenderer.color = color;
            yield return new WaitForSeconds(0.05f);
            spriteRenderer.color = originalColor;
            yield return new WaitForSeconds(0.05f);
            timer += 0.2f;
        }
        spriteRenderer.color = originalColor; // Ensure the original color is restored at the end
    }
}
