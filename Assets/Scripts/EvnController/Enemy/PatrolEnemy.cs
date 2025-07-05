using UnityEngine;
using System.Collections;

public class PatrolEnemy : BaseEnemy
{
    protected override void Patrol()
    {
        if (!canMove) return;

        Vector2 dir = facingLeft ? Vector2.left : Vector2.right;
        transform.Translate(dir * moveSpeed * Time.deltaTime);
    }

    protected override void TryAttack()
    {
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        if (stateInfo.IsName("Attack1") || stateInfo.IsName("Attack2")) return;

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
