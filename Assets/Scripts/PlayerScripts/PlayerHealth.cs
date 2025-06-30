using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Settings")]
    [SerializeField] private int maxHealth = 5;
    private int currentHealth;
    private bool isDead = false;

    private Animator animator;

    // ✅ Thuộc tính public để các script khác truy cập
    public bool IsDead => isDead;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        currentHealth -= damage;
        animator?.SetTrigger("Hurt");

        Debug.Log($"Player bị thương: -{damage} HP, còn lại: {currentHealth} HP");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        if (isDead) return;

        isDead = true;
        animator?.SetTrigger("Die");
        Debug.Log("Player đã chết.");
        // Thêm logic xử lý khi chết tại đây (tắt điều khiển, animation, v.v.)
    }
}
