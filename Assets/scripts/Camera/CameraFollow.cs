using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform player;

    private void LateUpdate()
    {
        transform.position = new Vector3(
            player.position.x+5,
            transform.position.y,
            transform.position.z
        );
    }
}