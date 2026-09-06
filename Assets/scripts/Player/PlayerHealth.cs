using System.Collections;
using UnityEngine;
using Manager;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] public int maxHP = 5;  // Maximum health points for the player
    public int currentHP;  // Current health points of the player

    [SerializeField] private float invincibleTime = 2f; // Duration of invincibility after taking damage
    public float InvincibleTimeNum => invincibleTime;
    private bool isInvincible = false; // Flag to track if the player is currently invincible
    private PlayerInvincibleEffect playerEffect; // Reference to the PlayerInvincibleEffect script for visual feedback
    private PlayerKnockback playerKnockback; // Reference to the PlayerKnockback script for knockback effect
    //--------------------------------------------------------------------------------------------------------------------------------------------
    //* プレーヤーエフェクトの取得
    private void Awake()
    {
        playerEffect = GetComponent<PlayerInvincibleEffect>();
        playerKnockback = GetComponent<PlayerKnockback>();
    }
    //* プレイヤーのHP初期化処理
    private void Start()
    {
        if (GameManager.Instance != null && GameManager.Instance.IsDebugMode())
        {  // Set the player's HP to 1 for testing purposes in debug mode
            currentHP = GameManager.Instance.DebugModeHP;
            Debug.Log($"Debug mode active. Player HP set to {currentHP}.");  //! Debug log to indicate that debug mode is active and the player's HP has been set. Should be deleted at launch.
        }
        else
        {
            currentHP = maxHP;
        }
    }
    // * プレイヤーがダメージを受けたときの処理
    public void TakeDamage(int damage, Transform attacker)
    {
        if (isInvincible)
        {
            return;
        }

        currentHP -= damage;    // Reduce current health by the damage amount

        Vector2 direction =
            (transform.position - attacker.position).normalized;  // Calculate the direction from the attacker to the player()

        playerKnockback.Knockback(direction);  // Apply knockback effect to the player in the calculated direction

        StartCoroutine(InvincibleTime());  // Start the invincibility coroutine after taking damage

        playerEffect.PlayInvincibleEffect(invincibleTime);  // Play the invincible flash effect

        if (GameManager.Instance != null && GameManager.Instance.IsDebugMode())
        {
            Debug.Log($"Debug mode active. Player took damage. Current HP: {currentHP}");   // Debug log to check the current HP after taking damage. Should be deleted at launch.
        }
        else
        {
            Debug.Log($"Player took damage. Current HP: {currentHP}");   // Debug log to check the current HP after taking damage. Should be deleted at launch.
        }

        if (currentHP <= 0)
        {
            Die();  // Call the Die method when the player's health reaches zero or below
        }
    }
    //* プレイヤーがダメージを受けた後の無敵時間処理
    private IEnumerator InvincibleTime()
    {
        isInvincible = true;
        yield return new WaitForSeconds(invincibleTime);
        isInvincible = false;
    }

    //* プレイヤーが死亡したときの処理
    private void Die()
    {
        Debug.Log("Player has died.");  // Debug log to indicate that the player has died. Should be deleted at launch.
        GameManager.Instance.GameOver();  // Call the GameOver method from the GameManager to handle game over logic
        Destroy(gameObject);  // Destroy the player game object
    }

    //*================================
    //* プレイヤーのHP回復処理
    //*================================
    public void Heal(int amount)
    {
        currentHP = Mathf.Min(currentHP + amount, maxHP);
        Debug.Log($"Player healed.\nHealed amount: {amount}\nCurrent HP: {currentHP}");   // Debug log to check the current HP after healing. Should be deleted at launch.
    }
}
