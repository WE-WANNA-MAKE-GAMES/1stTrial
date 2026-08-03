using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovements : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 10f;
    private Rigidbody2D rb;
    private PlayerControls controls;
    private Vector2 moveInput;
    private PlayerKnockback playerKnockback;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        controls = new PlayerControls();
        playerKnockback = GetComponent<PlayerKnockback>();
    }

    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }

    private void Update()
    {
        moveInput = controls.Player.Move.ReadValue<Vector2>();
    }

    private void FixedUpdate()
    {
        if (playerKnockback.IsKnockback)
        {
            return; // Skip movement if the player is being knocked back
        }

        rb.linearVelocity = moveInput.normalized * moveSpeed;
    }
}