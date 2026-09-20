using UnityEngine;

public class PlayerPiercing : MonoBehaviour
{
    [SerializeField] private int maxAdditionalPenetrations = 2;

    // 0: 通常弾（1体で消滅）
    // 1: 合計2体に命中可能
    private int additionalPenetrations = 0;

    public int AdditionalPenetrations => additionalPenetrations;

    public void AddPenetration(int amount)
    {
        additionalPenetrations = Mathf.Clamp(
            additionalPenetrations + amount,
            0,
            maxAdditionalPenetrations
        );
    }
}