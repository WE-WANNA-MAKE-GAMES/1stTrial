using System.Collections;
using UnityEngine;

public class NKFlash : MonoBehaviour
{
    [SerializeField] private float blinkIntervalStart = 0.5f;
    [SerializeField] private float blinkIntervalEnd = 0.05f;

    private SpriteRenderer spriteRenderer;
    private Color originalColor;

    private Coroutine flashCoroutine;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;
    }

    public void StartFlash(float duration)
    {
        if (flashCoroutine != null)
        {
            StopCoroutine(flashCoroutine);
        }

        flashCoroutine = StartCoroutine(Flash(duration));
    }

    private IEnumerator Flash(float duration)
    {
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            float progress = elapsedTime / duration;

            float interval = Mathf.Lerp(
                blinkIntervalStart,
                blinkIntervalEnd,
                progress
            );

            spriteRenderer.color = Color.white;

            yield return new WaitForSeconds(interval);

            spriteRenderer.color = originalColor;

            yield return new WaitForSeconds(interval);

            elapsedTime += interval * 2f;
        }

        spriteRenderer.color = originalColor;
        flashCoroutine = null;
    }
}