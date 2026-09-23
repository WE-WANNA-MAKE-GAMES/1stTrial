using UnityEngine;

public class EnemyBuffReceiver : MonoBehaviour
{
    public float SpeedMultiplier { get; private set; } = 1f;

    public void SetSpeedMultiplier(float multiplier)
    {
        SpeedMultiplier = Mathf.Max(1f, multiplier);
    }
}
