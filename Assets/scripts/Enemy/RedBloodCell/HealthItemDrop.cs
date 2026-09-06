using UnityEngine;

public class HealthItemDrop : MonoBehaviour
{
    [SerializeField] private GameObject healthItemPrefab;
    [Range(0f, 1f)]
    [SerializeField] private float dropChance = 0.3f;

    public void Drop()
    {
        if (healthItemPrefab == null || Random.value > dropChance) return;

        Instantiate(healthItemPrefab, transform.position, Quaternion.identity);
        Debug.Log($"Health item dropped at position: {transform.position}");
    }
}