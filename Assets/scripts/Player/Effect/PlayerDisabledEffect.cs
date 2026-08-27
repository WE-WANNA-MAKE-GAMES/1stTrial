using System.Collections;
using UnityEngine;

public class PlayerDisabledEffect : MonoBehaviour
{
    [SerializeField]
    private Color disabledColor = new Color(0.9f, 0.9f, 1f, 0.6f); // スタン中の色（やや白く半透明）

    private SpriteRenderer spriteRenderer;
    private Color originalColor;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;
    }

    public void PlayDisabledEffect(float duration)
    {
        StopAllCoroutines();
        StartCoroutine(DisabledEffect(duration));
    }

    private IEnumerator DisabledEffect(float duration)
    {
        spriteRenderer.color = disabledColor;

        yield return new WaitForSeconds(duration);

        spriteRenderer.color = originalColor;
    }
}