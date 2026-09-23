using UnityEngine;

public class SpeedItemDrop : MonoBehaviour
{
    [SerializeField] private GameObject speedItemPrefab;

    [Range(0f, 1f)]
    [SerializeField] private float dropChance = 0.3f;

    public void Drop()
    {
        if (speedItemPrefab == null || Random.value > dropChance) return;

        Instantiate(speedItemPrefab, transform.position, Quaternion.identity);
        Debug.Log($"Speed item dropped at position: {transform.position}");
    }
}