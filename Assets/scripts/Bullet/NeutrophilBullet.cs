using UnityEngine;

public class NeutrophilBullet : MonoBehaviour
{
    public float speed = -15f;
    [SerializeField] float destroyDistance = 15f;
    private CameraScroll cameraScroll;

    private void Awake()
    {
        cameraScroll = Camera.main.GetComponent<CameraScroll>();
    }

    void Update()
    {
        transform.localPosition += Vector3.right * speed * Time.deltaTime;

        if (transform.position.x < Camera.main.transform.position.x - destroyDistance)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("PlayerDamageReceiver") || other.CompareTag("Player"))
        {
            PlayerHealth playerHealth = other.GetComponentInParent<PlayerHealth>();
            if (playerHealth != null) playerHealth.TakeDamage(1, transform);
            Destroy(gameObject);
        }
        else if (other.CompareTag("Bullet"))
        {
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
    }
}