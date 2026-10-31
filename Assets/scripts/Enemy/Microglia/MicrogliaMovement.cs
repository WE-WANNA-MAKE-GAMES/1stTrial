using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class MicrogliaMovement : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 2.5f;

    [SerializeField]
    private float stopDistance = 0.8f;

    private Transform player;
    private Rigidbody2D rb;

    private void Awake()
    {
        gameObject.tag = "Enemy";

        if (GetComponent<EnemyHealth>() == null)
        {
            gameObject.AddComponent<EnemyHealth>();
        }

        if (GetComponent<EnemyEffect>() == null)
        {
            gameObject.AddComponent<EnemyEffect>();
        }

        if (GetComponent<EnemyKnockback>() == null)
        {
            gameObject.AddComponent<EnemyKnockback>();
        }

        if (GetComponent<MicrogliaContactDamage>() == null)
        {
            gameObject.AddComponent<MicrogliaContactDamage>();
        }

        if (GetComponent<Collider2D>() == null)
        {
            BoxCollider2D boxCollider = gameObject.AddComponent<BoxCollider2D>();
            boxCollider.isTrigger = true;
        }

        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (player == null)
        {
            GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
            if (playerObject != null)
            {
                player = playerObject.transform;
            }
            else
            {
                rb.linearVelocity = Vector2.zero;
                return;
            }
        }

        Vector2 direction = (Vector2)player.position - rb.position;
        if (direction.magnitude <= stopDistance)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        Vector2 targetVelocity = direction.normalized * moveSpeed;
        targetVelocity.x = Mathf.Min(targetVelocity.x, 0f);
        rb.linearVelocity = targetVelocity;
    }
}
