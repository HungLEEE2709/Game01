using UnityEngine;
using System.Collections;

public class FlyingEye : BaseEnemy
{
    [Header("Flying Eye Settings")]
    [SerializeField] private float dashForce = 10f;
    [SerializeField] private float dashDuration = 0.3f;

    private bool isDashing = false;
    private Rigidbody2D rb;

    protected override void Start()
    {
        base.Start();
        rb = GetComponent<Rigidbody2D>();
    }

    protected override void Patrol()
    {
        if (!canMove || isDashing || isAttacking) return;

        Vector2 dir = facingLeft ? Vector2.left : Vector2.right;
        transform.Translate(dir * moveSpeed * Time.deltaTime);
    }

    protected override void TryAttack()
    {
        if (isAttacking || isDashing || isDead) return;

        canMove = false;
        isAttacking = true;

        bool useAttack1 = Random.Range(0, 2) == 0;
        if (useAttack1)
            animator.SetTrigger("Attack1");
        else
            animator.SetTrigger("Attack2");

        StartCoroutine(PerformDashAttack());
    }

    private IEnumerator PerformDashAttack()
    {
        yield return new WaitForSeconds(0.2f); 

        if (isDead) yield break;

        isDashing = true;

        Vector2 direction = (player.position - transform.position).normalized;
        rb.linearVelocity = direction * dashForce;

        yield return new WaitForSeconds(dashDuration);

        rb.linearVelocity = Vector2.zero;
        isDashing = false;
        isAttacking = false;
        canMove = true;
        cooldownTimer = attackCooldown;

        animator.SetBool("isMoving", false);
    }

    public override void TakeDamage(int dmg = 1)
    {
        if (isDead) return;

        base.TakeDamage(dmg);

        isDashing = false;
        rb.linearVelocity = Vector2.zero;
    }

    public void EndHurt() 
    {
        if (!isDead)
        {
            canMove = true;
            isAttacking = false;
            isDashing = false;
        }
    }

    public void EndAttack() 
    {
        isAttacking = false;
        canMove = true;
        cooldownTimer = attackCooldown;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("EdgeStop"))
        {
            Flip();
            StartCoroutine(PauseBeforeContinue());
        }
    }

    private IEnumerator PauseBeforeContinue()
    {
        bool wasMoving = canMove;
        canMove = false;
        yield return new WaitForSeconds(1f);
        canMove = wasMoving;
    }
}
