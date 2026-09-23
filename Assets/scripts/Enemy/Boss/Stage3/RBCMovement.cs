using UnityEngine;

public class RBCMovement : MonoBehaviour
{
    [SerializeField] private float maxMoveSpeed = 5f;
    [SerializeField] private float minMoveSpeed = 2f;
    [SerializeField] float destroyDistance = 10f;

    private float moveSpeed;

    private void Awake() => moveSpeed = Random.Range(minMoveSpeed, maxMoveSpeed);

    public Vector2 Velocity => Vector2.left * GetMoveSpeed();

    private void Update()
    {
        transform.localPosition += Vector3.left * GetMoveSpeed() * Time.deltaTime;

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