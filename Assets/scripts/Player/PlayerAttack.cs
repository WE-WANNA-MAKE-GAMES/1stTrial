using UnityEngine;
using Manager;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private float attackPower = 1f;

    private float baseAttackPower;
    private float attackPowerMultiplier = 1f;

    private void Awake()
    {
        baseAttackPower = attackPower;
    }

    private void Start()
    {
        if (GameManager.Instance != null &&
            GameManager.Instance.IsDebugMode())
        {
            baseAttackPower =
                GameManager.Instance.DebugModeAttackPower;

            attackPower = baseAttackPower;
        }
    }

    public float AttackPower =>
        baseAttackPower * attackPowerMultiplier;

    public void ApplyAttackPowerDebuff(
        float multiplier,
        float duration)
    {
        attackPowerMultiplier = multiplier;

        CancelInvoke(nameof(RemoveAttackPowerDebuff));
        Invoke(
            nameof(RemoveAttackPowerDebuff),
            duration
        );
    }

    private void RemoveAttackPowerDebuff()
    {
        attackPowerMultiplier = 1f;
    }
}