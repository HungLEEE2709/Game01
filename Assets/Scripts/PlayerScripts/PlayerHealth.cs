using UnityEngine;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    public HealthBarUI healthBarUI;
    public int maxHealth = 5;
    private int currentHealth;
    private Animator animator;

    public bool IsDead { get; private set; } = false;
    public bool IsBlocking { get; set; } = false;
    private bool isInvincible = false;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        currentHealth = maxHealth;
        healthBarUI.UpdateHealth(currentHealth, maxHealth);
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
        healthBarUI.UpdateHealth(currentHealth, maxHealth);

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
        IsDead = true;
        animator.SetTrigger("Die");
        Debug.Log("Player đã chết!");
        GameManager.Instance?.GameOver();
    }
}
