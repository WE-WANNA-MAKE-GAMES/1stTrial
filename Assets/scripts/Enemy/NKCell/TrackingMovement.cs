using UnityEngine;

public class TrackMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float turnSpeed = 120f;
    [SerializeField] private CameraScroll cameraScroll;

    private Transform player;

    // 画面上での現在の進行方向
    private Vector2 currentDirection = Vector2.left;

    // NK自身のlocal移動速度
    public Vector2 Velocity { get; private set; }

    private void Awake()
    {
        GameObject playerObject =
            GameObject.FindGameObjectWithTag("Player");

        if (playerObject != null)
        {
            player = playerObject.transform;
        }

        if (cameraScroll == null)
        {
            cameraScroll = FindAnyObjectByType<CameraScroll>();
        }
    }

    private void Update()
    {
        if (player == null)
        {
            Move(currentDirection);
            return;
        }

        // プレイヤーへの方向
        Vector2 targetDirection =
            (player.position - transform.position).normalized;

        // 現在の角度
        float currentAngle =
            Mathf.Atan2(
                currentDirection.y,
                currentDirection.x
            ) * Mathf.Rad2Deg;

        // プレイヤー方向の角度
        float targetAngle =
            Mathf.Atan2(
                targetDirection.y,
                targetDirection.x
            ) * Mathf.Rad2Deg;

        // 1フレームで曲がれる最大角度
        float maxAngle =
            turnSpeed * Time.deltaTime;

        // 現在方向からプレイヤー方向へ徐々に旋回
        float newAngle =
            Mathf.MoveTowardsAngle(
                currentAngle,
                targetAngle,
                maxAngle
            );

        // 新しい画面上の進行方向
        currentDirection =
            new Vector2(
                Mathf.Cos(newAngle * Mathf.Deg2Rad),
                Mathf.Sin(newAngle * Mathf.Deg2Rad)
            ).normalized;

        Move(currentDirection);
    }

    private void Move(Vector2 direction)
    {
        // 画面上でのNKの目標速度
        Vector2 targetVelocity =
            direction * moveSpeed;

        // ScrollRootによる移動速度
        Vector2 scrollVelocity = Vector2.zero;

        if (cameraScroll != null)
        {
            scrollVelocity =
                Vector2.right * cameraScroll.ScrollSpeed;
        }

        // ScrollRootの移動を考慮して
        // NK自身に必要なlocal速度を計算
        Vector2 localVelocity =
            targetVelocity - scrollVelocity;

        Velocity = localVelocity;

        transform.localPosition +=
            (Vector3)(localVelocity * Time.deltaTime);
    }
}