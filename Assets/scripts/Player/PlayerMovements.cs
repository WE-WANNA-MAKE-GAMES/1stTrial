using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovements : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 10f;
    private Rigidbody2D rb;
    private PlayerControls controls;
    private Vector2 moveInput;
    private PlayerKnockback playerKnockback;

    private bool isDisabled = false;
    private Coroutine disableMovementCoroutine;
    private PlayerInvincibleEffect playerInvincibleEffect;
    private PlayerDisabledEffect playerDisabledEffect;
    private PlayerHealth playerHealth;
    [SerializeField] private CameraScroll cameraScroll;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        controls = new PlayerControls();
        playerKnockback = GetComponent<PlayerKnockback>();
        playerHealth = GetComponent<PlayerHealth>();
        playerDisabledEffect = GetComponent<PlayerDisabledEffect>();
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
        if (!(playerKnockback.IsKnockback || isDisabled))
        {
            Vector2 playerVelocity = moveInput.normalized * moveSpeed;

            // カメラがスクロールしている間だけ、
            // プレイヤーにもステージの移動速度を加える
            if (!cameraScroll.IsAtStageEnd)
            {
                playerVelocity.x += cameraScroll.ScrollSpeed;
            }

            rb.linearVelocity = playerVelocity;
        }

        // ノックバック中・行動不能中も画面内に留める
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

    public void SetDisabled(float duration)
    {
        if (disableMovementCoroutine != null)
        {
            StopCoroutine(disableMovementCoroutine);
        }

        disableMovementCoroutine = StartCoroutine(DisableMovement(duration));
    }

    private IEnumerator DisableMovement(float duration)
    {
        isDisabled = true;

        float holdX = cameraScroll.IsAtStageEnd ? 0f : cameraScroll.ScrollSpeed;
        rb.linearVelocity = new Vector2(holdX, 0f);

        yield return new WaitForSeconds(duration);

        isDisabled = false;
        disableMovementCoroutine = null;
    }
}