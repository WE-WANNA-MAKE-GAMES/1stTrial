using UnityEngine;

public class PiercingItemDrop : MonoBehaviour
{
    [SerializeField] private GameObject piercingItemPrefab;

    [Range(0f, 1f)]
    [SerializeField] private float dropChance = 0.15f;

    public void Drop()
    {
        if (piercingItemPrefab == null || Random.value > dropChance)
        {
            return;
        }

        Instantiate(
            piercingItemPrefab,
            transform.position,
            Quaternion.identity
        );
    }
}