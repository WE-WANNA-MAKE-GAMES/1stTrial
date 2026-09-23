using UnityEngine;

public class SpreadShotItemDrop : MonoBehaviour
{
    [SerializeField] private GameObject spreadShotItemPrefab;

    [Range(0f, 1f)]
    [SerializeField] private float dropChance = 0.1f;

    public void Drop()
    {
        if (spreadShotItemPrefab == null || Random.value > dropChance)
        {
            return;
        }

        Instantiate(
            spreadShotItemPrefab,
            transform.position,
            Quaternion.identity
        );
    }
}