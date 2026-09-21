using UnityEngine;

public class PiercingItem : MonoBehaviour
{
    [SerializeField] private int addPenetrations = 1;

    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerPiercing playerPiercing =
            other.GetComponentInParent<PlayerPiercing>();

        if (playerPiercing == null) return;

        playerPiercing.AddPenetration(addPenetrations);
        Destroy(gameObject);
    }
}