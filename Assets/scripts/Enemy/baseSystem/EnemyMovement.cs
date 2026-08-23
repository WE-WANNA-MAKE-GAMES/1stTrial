using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] float destroyDistance = 10f;

    private void Update()
    {
        transform.localPosition += Vector3.left * moveSpeed * Time.deltaTime;

        // Destroy when off-screen (world座標で判定するのでここは変更なし)
        if (transform.position.x < Camera.main.transform.position.x - destroyDistance)
        {
            Destroy(gameObject);
        }
    }
}