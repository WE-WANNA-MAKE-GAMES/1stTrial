using UnityEngine;

public class SpreadShotItem : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerSpreadShot spreadShot =
            other.GetComponentInParent<PlayerSpreadShot>();

        if (spreadShot == null) return;

        spreadShot.Activate();
        Destroy(gameObject);
    }
}