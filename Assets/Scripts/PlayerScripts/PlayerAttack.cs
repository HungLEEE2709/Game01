using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [Header("Combo Settings")]
    [SerializeField] private float comboResetTime = 1.0f;

    [Header("Attack Settings")]
    [SerializeField] private Transform attackPoint;
    [SerializeField] private float attackRange = 1f;
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private int damage = 1;

    private Animator animator;
    private Rigidbody2D rb;

    private int currentAttack = 0;
    private float lastAttackTime = 0f;
    private bool isAttacking = false;

    public bool IsAttacking => isAttacking;
    public int CurrentAttack => currentAttack;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    public void HandleAttack()
    {
        if (isAttacking) return;

        if (Input.GetMouseButtonDown(0))
        {
            float timeSinceLastAttack = Time.time - lastAttackTime;

            if (timeSinceLastAttack > comboResetTime)
            {
                currentAttack = 1;
            }
            else
            {
                currentAttack++;
                if (currentAttack > 3)
                    currentAttack = 1;
            }

            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);

            animator.ResetTrigger("Attack1");
            animator.ResetTrigger("Attack2");
            animator.ResetTrigger("Attack3");
            animator.SetTrigger("Attack" + currentAttack);

            isAttacking = true;
            lastAttackTime = Time.time;
        }
    }

    public void EndAttack() // Gọi từ Animation Event cuối mỗi đòn
    {
        isAttacking = false;
    }

    public void DealDamage() // Gọi từ Animation Event ở frame ra đòn
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayer);

        foreach (Collider2D enemy in hitEnemies)
        {
            if (enemy.TryGetComponent(out PatrolEnemy patrolEnemy))
            {
                patrolEnemy.TakeDamage(damage);
                Debug.Log("Đánh trúng enemy!");
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
