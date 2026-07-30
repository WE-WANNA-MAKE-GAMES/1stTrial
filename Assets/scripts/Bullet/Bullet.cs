using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 10f;
    void Update()
    {
        transform.position += Vector3.right * speed * Time.deltaTime;
    }
    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
