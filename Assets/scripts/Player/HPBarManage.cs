using UnityEngine;
using UnityEngine.UI;

public class HPBarManage : MonoBehaviour
{
    [SerializeField] private Image health;
    private PlayerHealth playerHealth;

    private void Awake()
    {
        if (health == null)
        {
            health = GetComponent<Image>();
        }

        playerHealth = GetComponentInParent<PlayerHealth>();
    }

    void Update()
    {
        if (health == null || playerHealth == null || playerHealth.maxHP <= 0)
        {
            return;
        }

        health.fillAmount = (float)playerHealth.currentHP / playerHealth.maxHP;
    }
}
