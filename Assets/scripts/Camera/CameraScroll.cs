using UnityEngine;

public class CameraScroll : MonoBehaviour
{
    [SerializeField]
    private float scrollSpeed = 3f;

    [SerializeField]
    private BoxCollider2D stageBounds;

    public float ScrollSpeed => scrollSpeed;

    public bool IsAtStageEnd { get; private set; }

    private float stageRightX;

    private void Awake()
    {
        Camera cam = GetComponent<Camera>();

        if (stageBounds == null)
        {
            Debug.LogError("Stage Bounds is not assigned.");
            return;
        }

        float cameraHalfWidth =
            cam.orthographicSize * cam.aspect;

        stageRightX =
            stageBounds.bounds.max.x - cameraHalfWidth;
    }

    private void FixedUpdate()
    {
        Scroll();
    }

    private void Scroll()
    {
        if (IsAtStageEnd)
        {
            return;
        }

        float nextX =
            transform.position.x +
            scrollSpeed * Time.fixedDeltaTime;

        if (nextX >= stageRightX)
        {
            nextX = stageRightX;
            IsAtStageEnd = true;
        }

        transform.position = new Vector3(
            nextX,
            transform.position.y,
            transform.position.z
        );
    }
}