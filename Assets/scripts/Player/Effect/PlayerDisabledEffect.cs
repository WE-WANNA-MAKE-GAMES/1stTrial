using System.Collections;
using UnityEngine;

public class PlayerDisabledEffect : MonoBehaviour
{
    [SerializeField]
    private float whiteAmount = 0.5f;

    private SpriteRenderer spriteRenderer;
    private Color originalColor;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;
    }

    public void PlayDisabledEffect(float duration)
    {
        StartCoroutine(DisabledEffect(duration));
    }

    private IEnumerator DisabledEffect(float duration)
    {
        Color disabledColor = Color.Lerp(
            originalColor,
            Color.white,
            whiteAmount
        );

        spriteRenderer.color = disabledColor;

        yield return new WaitForSeconds(duration);

        spriteRenderer.color = originalColor;
    }
}