using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private float comboResetTime = 1.0f;

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


    public void EndAttack()
    {
        isAttacking = false;
    }
}
