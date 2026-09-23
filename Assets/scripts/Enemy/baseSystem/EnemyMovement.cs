using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] float destroyDistance = 10f;

    public Vector2 Velocity => Vector2.left * GetMoveSpeed();

    private void Update()
    {
        transform.localPosition += Vector3.left * GetMoveSpeed() * Time.deltaTime;

        if (Camera.main == null)
        {
            return;
        }

        if (transform.position.x < Camera.main.transform.position.x - destroyDistance)
        {
            Destroy(gameObject);
        }
    }

    private float GetMoveSpeed()
    {
        EnemyBuffReceiver receiver = GetComponent<EnemyBuffReceiver>();
        return moveSpeed * (receiver != null ? receiver.SpeedMultiplier : 1f);
    }
}