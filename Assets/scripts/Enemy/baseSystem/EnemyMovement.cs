using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 5f;

    private CameraScroll cameraScroll;

    private void Awake()
    {
        cameraScroll = Camera.main.GetComponent<CameraScroll>();
    }

    private void Update()
    {
        if (cameraScroll == null)
        {
            return;
        }

        float currentSpeed = moveSpeed;

        // カメラがスクロールしている間だけ
        // スクロール分を敵の移動速度に加える
        if (cameraScroll.IsAtStageEnd)
        {
            currentSpeed += cameraScroll.ScrollSpeed;
        }

        transform.position +=
            Vector3.left * currentSpeed * Time.deltaTime;
    }
}