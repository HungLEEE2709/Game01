using UnityEngine;
using System.Collections;

public class PatrolEnemy : MonoBehaviour
{
    [Header("Patrol Settings")]
    public float moveSpeed = 2f;
    private bool facingLeft = true;
    private bool canMove = true;

    [Header("Attack Settings")]
    public float detectRange = 3f;
    public float attackCooldown = 2f;
    public float attackRange = 1.5f;
    public int damage = 1;
    public Transform player;

    [Header("Health")]
    public int maxHealth = 3;
    private int currentHealth;
    private bool isDead = false;

    private float cooldownTimer;
    private Animator animator;

    private void Start()
    {
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();

        if (player == null)
        {
            GameObject foundPlayer = GameObject.FindGameObjectWithTag("Player");
            if (foundPlayer != null)
                player = foundPlayer.transform;
        }
    }

    private void Update()
    {
        if (player == null || isDead) return;

        cooldownTimer -= Time.deltaTime;
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer <= detectRange)
        {
            if ((player.position.x < transform.position.x && !facingLeft) ||
                (player.position.x > transform.position.x && facingLeft))
            {
                Flip();
            }

            if (cooldownTimer <= 0f)
            {
                Attack();
                cooldownTimer = attackCooldown;
            }
            else
            {
                if (canMove)
                {
                    Vector2 direction = (player.position - transform.position).normalized;
                    transform.Translate(new Vector2(direction.x, 0f) * moveSpeed * Time.deltaTime);
                }
                ResetAttack();
            }
        }
        else
        {
            ResetAttack();
            Patrol();
        }
    }

    private void Patrol()
    {
        if (!canMove) return;

        Vector2 moveDir = facingLeft ? Vector2.left : Vector2.right;
        transform.Translate(moveDir * moveSpeed * Time.deltaTime);
    }

    public void TakeDamage(int damage = 1)
    {
        if (isDead) return;

        currentHealth -= damage;
        animator.SetTrigger("Hurt");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Attack()
    {
        ResetAttack();

        bool useAttack1 = Random.Range(0, 2) == 0;

        if (useAttack1)
        {
            animator.SetBool("Attack1", true);
            animator.SetBool("Attack2", false);
        }
        else
        {
            animator.SetBool("Attack1", false);
            animator.SetBool("Attack2", true);
        }

        Debug.Log("Enemy attacks: " + (useAttack1 ? "Attack1" : "Attack2"));
    }

    public void DealDamage() // Gọi từ Animation Event
    {
        if (isDead || player == null) return;

        float distance = Vector2.Distance(transform.position, player.position);
        if (distance <= attackRange)
        {
            PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
            if (playerHealth != null && !playerHealth.IsDead)
            {
                playerHealth.TakeDamage(damage);
                Debug.Log("Enemy gây sát thương cho Player!");
            }
        }
    }

    private void ResetAttack()
    {
        animator.SetBool("Attack1", false);
        animator.SetBool("Attack2", false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("EdgeStop"))
        {
            StartCoroutine(PauseAndFlip());
        }
    }

    private IEnumerator PauseAndFlip()
    {
        canMove = false;

        if (animator != null)
            animator.SetBool("Idle", true);

        yield return new WaitForSeconds(1f);

        if (animator != null)
            animator.SetBool("Idle", false);

        Flip();

        canMove = true;
    }

    private void Flip()
    {
        facingLeft = !facingLeft;

        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    private void Die()
    {
        isDead = true;
        canMove = false;

        animator.SetTrigger("Die");

        Collider2D col = GetComponent<Collider2D>();
        if (col) col.enabled = false;

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb) rb.linearVelocity = Vector2.zero;

        Destroy(gameObject, 2f);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
