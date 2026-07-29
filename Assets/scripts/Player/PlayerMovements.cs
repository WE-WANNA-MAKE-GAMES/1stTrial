using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 5f;

    private Rigidbody2D rb;
    private PlayerControls controls;
    private Vector2 moveInput;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        controls = new PlayerControls();
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
        rb.linearVelocity = moveInput.normalized * moveSpeed;
    }
    private void LateUpdate()
{
    Vector3 pos = transform.position;

    pos.x = Mathf.Clamp(pos.x, -8.5f, 8.5f);
    pos.y = Mathf.Clamp(pos.y, -4.5f, 4.5f);

    transform.position = pos;
}
}