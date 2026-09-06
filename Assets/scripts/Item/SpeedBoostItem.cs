using UnityEngine;

public class SpeedBoostItem : MonoBehaviour
{
    [SerializeField] private float speedMultiplier = 1.2f;
    [SerializeField] private float duration = 10f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerMovements playerMovement =
            other.GetComponentInParent<PlayerMovements>();

        if (playerMovement == null) return;

        playerMovement.ApplySpeedBoost(speedMultiplier, duration);
        Destroy(gameObject);
    }
}