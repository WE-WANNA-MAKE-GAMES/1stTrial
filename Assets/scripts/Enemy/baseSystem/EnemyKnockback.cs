using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyKnockback : MonoBehaviour
{
    [SerializeField] private float knockbackPower = 5f;

    [SerializeField] private float knockbackTime = 0.1f;

    private Rigidbody2D rb;

    public bool IsKnockback { get; private set; }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Knockback(Vector2 direction)
    {
        StopAllCoroutines();
        StartCoroutine(KnockbackCoroutine(direction));
    }

    private IEnumerator KnockbackCoroutine(Vector2 direction)
    {
        IsKnockback = true;
        rb.linearVelocity = direction.normalized * knockbackPower;
        yield return new WaitForSeconds(knockbackTime);
        IsKnockback = false;
    }
}