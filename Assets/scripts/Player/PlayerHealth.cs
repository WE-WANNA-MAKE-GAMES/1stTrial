using System.Collections;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int maxHP = 5;  // Maximum health points for the player
    private int currentHP;  // Current health points of the player

    [SerializeField] private float invincibleTime = 2f; // Duration of invincibility after taking damage
    private bool isInvincible = false; // Flag to track if the player is currently invincible
    private PlayerInvincibleEffect playerEffect; // Reference to the PlayerInvincibleEffect script for visual feedback

    //--------------------------------------------------------------------------------------------------------------------------------------------
    //* プレーヤーエフェクトの取得
    private void Awake()
    {
        playerEffect = GetComponent<PlayerInvincibleEffect>();
    }
    //* プレイヤーのHP初期化処理
    private void Start()
    {
        currentHP = maxHP;  // Initialize current health to maximum health at the start
    }
    // * プレイヤーがダメージを受けたときの処理
    public void TakeDamage(int damage)
    {
        if (isInvincible)
        {
            return;
        }

        currentHP -= damage;    // Reduce current health by the damage amount

        StartCoroutine(InvincibleTime());  // Start the invincibility coroutine after taking damage

        playerEffect.PlayInvincibleEffect(invincibleTime);  // Play the invincible flash effect

        Debug.Log("Player took damage. Current HP: " + currentHP);   // Debug log to check the current HP after taking damage. Should be deleted at launch.
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
}
