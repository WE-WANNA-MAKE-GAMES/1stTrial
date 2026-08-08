using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovements : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 10f;
    private Rigidbody2D rb;
    private PlayerControls controls;
    private Vector2 moveInput;
    private PlayerKnockback playerKnockback;

    [SerializeField] private CameraScroll cameraScroll;

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
        Vector2 playerVelocity = moveInput.normalized * moveSpeed;
        playerVelocity.x += cameraScroll.ScrollSpeed; // プレイヤーの入力に依存した速度にカメラスピードを加える

        rb.linearVelocity = playerVelocity;
        ClampToCamera();
    }
    private void ClampToCamera()
    {
        Camera cam = Camera.main;

        Vector3 viewPos = cam.WorldToViewportPoint(transform.position);

        viewPos.x = Mathf.Clamp(viewPos.x, 0.05f, 0.95f);
        viewPos.y = Mathf.Clamp(viewPos.y, 0.05f, 0.95f);

        rb.position = cam.ViewportToWorldPoint(viewPos);
    }
}