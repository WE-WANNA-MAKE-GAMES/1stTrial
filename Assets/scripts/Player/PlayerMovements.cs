using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using Manager;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovements : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 10f;
    private Rigidbody2D rb;
    private PlayerControls controls;
    private Vector2 moveInput;
    private PlayerKnockback playerKnockback;

    private bool isDisabled = false;
    private PlayerDisabledEffect playerDisabledEffect;
    [SerializeField] private CameraScroll cameraScroll;

    private Coroutine speedBoostCoroutine;
    private float baseMoveSpeed;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        controls = new PlayerControls();
        playerKnockback = GetComponent<PlayerKnockback>();
        playerDisabledEffect = GetComponent<PlayerDisabledEffect>();
        baseMoveSpeed = moveSpeed;
    }

    private float GetMoveSpeed()
    {
        if (GameManager.Instance != null && GameManager.Instance.IsDebugMode())
        {
            return GameManager.Instance.DebugModeSpeed;
        }
        return moveSpeed;
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
        if (playerKnockback.IsKnockback || isDisabled)
        {
            return;
        }

        Vector2 playerVelocity = moveInput.normalized * GetMoveSpeed();

        // カメラがスクロールしている間だけ、
        // プレイヤーにもステージの移動速度を加える
        if (!cameraScroll.IsAtStageEnd)
        {
            playerVelocity.x += cameraScroll.ScrollSpeed;
        }

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

    public void SetDisabled(float duration)
    {
        StartCoroutine(DisableMovement(duration));
    }

    private IEnumerator DisableMovement(float duration)
    {
        isDisabled = true;

        float scrollCompensation = cameraScroll.IsAtStageEnd ? 0f : cameraScroll.ScrollSpeed;
        rb.linearVelocity = new Vector2(scrollCompensation, 0f);

        yield return new WaitForSeconds(duration);

        isDisabled = false;
    }

    public void ApplySpeedBoost(float multiplier, float duration)
    {
        if (speedBoostCoroutine != null)
        {
            StopCoroutine(speedBoostCoroutine);
        }

        moveSpeed = baseMoveSpeed * multiplier;
        speedBoostCoroutine = StartCoroutine(SpeedBoostDuration(duration));
    }

    private IEnumerator SpeedBoostDuration(float duration)
    {
        yield return new WaitForSeconds(duration);

        moveSpeed = baseMoveSpeed;
        speedBoostCoroutine = null;
    }
}