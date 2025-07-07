using UnityEngine;
using System.Collections;

public class PatrolEnemy : BaseEnemy
{
    protected override void Patrol()
    {
        if (!canMove || isAttacking || isDead) return;

        Vector2 dir = facingLeft ? Vector2.left : Vector2.right;
        transform.Translate(dir * moveSpeed * Time.deltaTime);
    }

    protected override void TryAttack()
    {
        if (isAttacking || isDead || cooldownTimer > 0f) return;

        canMove = false;
        isAttacking = true;

        bool attack1 = Random.Range(0, 2) == 0;
        if (attack1)
            animator.SetTrigger("Attack1");
        else
            animator.SetTrigger("Attack2");
    }

    public void EndAttack() 
    {
        canMove = true;
        isAttacking = false;
        cooldownTimer = attackCooldown;
    }

    public override void TakeDamage(int dmg = 1)
    {
        if (isDead) return;

        base.TakeDamage(dmg);

        isAttacking = false;
        canMove = false;

        animator.SetTrigger("Hurt");
    }

    public void EndHurt() 
    {
        if (!isDead)
        {
            canMove = true;
            isAttacking = false;
        }
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
