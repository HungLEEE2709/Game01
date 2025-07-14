using UnityEngine;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    public HealthBarUI healthBarUI;
    public int maxHealth = 5;
    private int currentHealth;
    private Animator animator;
    private GameManager gameManager;
    private AudioManager audioManager;

    public bool IsDead { get; private set; } = false;
    public bool IsBlocking { get; set; } = false;
    private bool isInvincible = false;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        gameManager = FindAnyObjectByType<GameManager>();
        audioManager = FindAnyObjectByType<AudioManager>();

        // ✅ Lấy HP từ GameManager nếu đã lưu, ngược lại dùng maxHealth
        if (gameManager != null)
        {
            currentHealth = gameManager.playerHealth > 0 ? gameManager.playerHealth : maxHealth;
        }
        else
        {
            currentHealth = maxHealth;
        }

        // ✅ Cập nhật thanh máu
        if (healthBarUI != null)
        {
            healthBarUI.UpdateHealth(currentHealth, maxHealth);
        }
    }

    private void Update()
    {
        // ✅ Rơi khỏi bản đồ thì chết
        if (!IsDead && transform.position.y < -6f)
        {
            Debug.Log("Player rơi xuống vực!");
            Die();
        }

        // ✅ Cập nhật HP mỗi frame vào GameManager để lưu khi chuyển màn
        if (gameManager != null && !IsDead)
        {
            gameManager.playerHealth = currentHealth;
        }
    }

    public void TakeDamage(int damage)
    {
        if (IsDead || isInvincible) return;

        if (IsBlocking)
        {
            Debug.Log("Đỡ đòn – không mất máu");
            return;
        }

        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        healthBarUI?.UpdateHealth(currentHealth, maxHealth);

        Debug.Log("Player bị mất máu! Máu còn lại: " + currentHealth);

        animator.ResetTrigger("Attack1");
        animator.ResetTrigger("Attack2");
        animator.ResetTrigger("Attack3");
        animator.SetTrigger("Hurt");

        GetComponent<PlayerAttack>()?.EndAttack();

        StartCoroutine(InvincibilityFrames(0.5f));

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private IEnumerator InvincibilityFrames(float duration)
    {
        isInvincible = true;
        yield return new WaitForSeconds(duration);
        isInvincible = false;
    }

    private void Die()
    {
        if (IsDead) return;

        IsDead = true;
        currentHealth = 0;
        animator.SetTrigger("Die");

        if (audioManager != null)
        {
            audioManager.StopMusic();
            audioManager.PlayDeathSound();
        }

        StartCoroutine(ShowGameOverAfterDelay());
    }

    private IEnumerator ShowGameOverAfterDelay()
    {
        yield return new WaitForSeconds(1.2f);

        if (gameManager != null)
        {
            gameManager.GameOver();
            Debug.Log("GameOver được gọi từ PlayerHealth!");
        }
        else
        {
            Debug.LogError("Không tìm thấy GameManager!");
        }
    }
}
