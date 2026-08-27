using System.Collections;
using UnityEngine;

public class PlayerInvincibleEffect : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Color originalColor;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;
    }

    public void PlayInvincibleEffect(float duration)
    {
        StopAllCoroutines();
        StartCoroutine(InvincibleFlash(duration));
    }

    private IEnumerator InvincibleFlash(float duration)
    {
        float timer = 0f;

        while (timer < duration)
        {
            Color color = originalColor;
            color.a = 0.3f;

            spriteRenderer.color = color;

            yield return new WaitForSeconds(0.05f);

            spriteRenderer.color = originalColor;

            yield return new WaitForSeconds(0.05f);

            timer += 0.1f;
        }

        spriteRenderer.color = originalColor;
    }
}