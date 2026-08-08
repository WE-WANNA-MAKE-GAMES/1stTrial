using UnityEngine;

public class CameraScroll : MonoBehaviour
{
    [SerializeField] private float scrollSpeed = 3f;    //カメラのスクロール速度
    public float ScrollSpeed => scrollSpeed;

    private void FixedUpdate()
    {
        Scroll();
    }

    private void Scroll()
    {
        //カメラのスクロール
        transform.position += Vector3.right * scrollSpeed * Time.deltaTime;
    }
}
