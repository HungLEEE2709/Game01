using UnityEngine;

public abstract class BaseEnemy : MonoBehaviour
{
    [Header("Base Settings")]
    public float moveSpeed = 2f;
    public int maxHealth = 5;
    public float detectRange = 3f;
    public float attackCooldown = 2f;
    public float attackRange = 1.5f;
    public int damage = 1;

    [Header("Player Target")]
    public Transform player;

    [Header("Drop Settings")]
    public GameObject coinPrefab;
    public int minCoinDrop = 1;
    public int maxCoinDrop = 3;

    protected int currentHealth;
    protected float cooldownTimer;
    protected bool facingLeft = true;
    protected bool canMove = true;
    protected bool isDead = false;
    protected bool isAttacking = false;

    protected Animator animator;

    protected virtual void Start()
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

    protected virtual void Update()
    {
        if (isDead || player == null) return;

        cooldownTimer -= Time.deltaTime;
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer <= detectRange)
        {
            FacePlayer();

            if (!isAttacking && cooldownTimer <= 0f)
            {
                TryAttack();
            }
            else if (canMove && !isAttacking)
            {
                MoveToPlayer();
            }
        }
        else
        {
            Patrol();
        }

        animator.SetBool("isMoving", canMove && IsMoving());
    }

    protected virtual void FacePlayer()
    {
        if ((player.position.x < transform.position.x && !facingLeft) ||
            (player.position.x > transform.position.x && facingLeft))
        {
            Flip();
        }
    }

    protected virtual void MoveToPlayer()
    {
        Vector2 direction = (player.position - transform.position).normalized;
        transform.Translate(new Vector2(direction.x, 0f) * moveSpeed * Time.deltaTime);
    }

    protected abstract void TryAttack();
    protected virtual void Patrol() { }

    public virtual void TakeDamage(int dmg = 1)
    {
        if (isDead) return;

        currentHealth -= dmg;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        Debug.Log($"{gameObject.name} bị đánh: HP còn lại {currentHealth}");

        canMove = false;
        animator.SetTrigger("Hurt");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void EndHurt()
    {
        canMove = true;
    }

    public virtual void DealDamage()
    {
        if (isDead || player == null) return;

        float distance = Vector2.Distance(transform.position, player.position);
        if (distance <= attackRange)
        {
            PlayerHealth hp = player.GetComponent<PlayerHealth>();
            if (hp != null && !hp.IsDead)
            {
                hp.TakeDamage(damage);
            }
        }
    }
    protected virtual void Flip()
    {
        facingLeft = !facingLeft;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    protected virtual void Die()
    {
        isDead = true;
        canMove = false;
        animator.SetTrigger("Die");

        DropCoins();

        Collider2D col = GetComponent<Collider2D>();
        if (col) col.enabled = false;

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb) rb.linearVelocity = Vector2.zero;

        Destroy(gameObject, 2f);
    }

    protected virtual void DropCoins()
    {
        if (coinPrefab == null) return;

        int coinCount = Random.Range(minCoinDrop, maxCoinDrop + 1);

        for (int i = 0; i < coinCount; i++)
        {
            Vector3 dropPos = transform.position + new Vector3(Random.Range(-0.3f, 0.3f), Random.Range(0.2f, 0.5f), 0);
            Instantiate(coinPrefab, dropPos, Quaternion.identity);
        }
    }

    protected virtual bool IsMoving()
    {
        return true;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
