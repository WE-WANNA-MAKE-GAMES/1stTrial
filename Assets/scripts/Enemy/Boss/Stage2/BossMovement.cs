using UnityEngine;

public class Stage2BossMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 1f;
    [SerializeField] private float moveRange = 2f;

    private float startY;
    private float time;

    private void Start()
    {
        startY = transform.position.y;
    }

    private void Update()
    {
        time += Time.deltaTime * moveSpeed;

        float offset =
            Mathf.Sin(time) * moveRange;

        Vector3 position = transform.position;
        position.y = startY + offset;

        transform.position = position;
    }
}