using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] float destroyDistance = 10f;

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