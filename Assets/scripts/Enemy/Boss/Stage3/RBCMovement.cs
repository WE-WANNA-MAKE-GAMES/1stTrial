using UnityEngine;

public class RBCMovement : MonoBehaviour
{
    [SerializeField] private float maxMoveSpeed = 5f;
    [SerializeField] private float minMoveSpeed = 2f;
    [SerializeField] float destroyDistance = 10f;

    private float moveSpeed;

    private void Awake() => moveSpeed = Random.Range(minMoveSpeed, maxMoveSpeed);

    public Vector2 Velocity => Vector2.left * moveSpeed; // 追加

    private void Update()
    {
        transform.localPosition += Vector3.left * moveSpeed * Time.deltaTime;

        if (transform.position.x < Camera.main.transform.position.x - destroyDistance)
        {
            Destroy(gameObject);
        }
    }
}