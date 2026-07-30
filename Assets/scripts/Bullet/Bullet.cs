using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 15f;
    void Update()
    {
        transform.position += Vector3.right * speed * Time.deltaTime;
    }
    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
    }
}
