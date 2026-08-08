using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyMovement : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 5f;

    private Rigidbody2D rb;
    private EnemyKnockback knockback;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        knockback = GetComponent<EnemyKnockback>();
    }

    private void FixedUpdate()
    {
        // ノックバック中は通常移動しない
        if (knockback != null && knockback.IsKnockback)
            return;

        rb.linearVelocity = Vector2.left * moveSpeed;
    }
}