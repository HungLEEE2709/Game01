using UnityEngine;
using System.Collections;

public class BossEnemy : BaseEnemy
{
    [Header("Spell Settings")]
    public GameObject fireBallPrefab;
    public GameObject iceBallPrefab;
    public Transform shootPoint;
    public float spellSpeed = 5f;
    public bool useMultiDirection = false; 

    private enum SpellType { Fire, Ice }
    private SpellType currentSpell;

    protected override void Start()
    {
        base.Start();
        currentSpell = SpellType.Fire;
    }

    protected override void Update()
    {
        if (isDead || player == null) return;

        cooldownTimer -= Time.deltaTime;

        float distance = Vector2.Distance(transform.position, player.position);

        if (distance <= detectRange)
        {
            FacePlayer();

            if (!isAttacking && cooldownTimer <= 0f)
            {
                TryAttack();
            }
        }
        else
        {
            Patrol();
        }

        animator.SetBool("isMoving", canMove && !isAttacking && !isDead);
    }

    protected override void TryAttack()
    {
        if (isAttacking) return;

        canMove = false;
        isAttacking = true;

        // Random phép
        currentSpell = (Random.Range(0, 2) == 0) ? SpellType.Fire : SpellType.Ice;

        animator.SetTrigger("Attack"); // animation chung
    }

    // Gọi từ Animation Event
    public void ShootSpell()
    {
        GameObject spellPrefab = (currentSpell == SpellType.Fire) ? fireBallPrefab : iceBallPrefab;

        if (useMultiDirection)
        {
            int directions = 5;
            float angleStep = 360f / directions;
            for (int i = 0; i < directions; i++)
            {
                float angle = angleStep * i;
                Quaternion rot = Quaternion.Euler(0, 0, angle);
                GameObject spell = Instantiate(spellPrefab, shootPoint.position, rot);
                Rigidbody2D rb = spell.GetComponent<Rigidbody2D>();
                Vector2 dir = rot * Vector2.right;
                rb.linearVelocity = dir * spellSpeed;
            }
        }
        else
        {
            Vector2 dir = (player.position - shootPoint.position).normalized;
            GameObject spell = Instantiate(spellPrefab, shootPoint.position, Quaternion.identity);
            Rigidbody2D rb = spell.GetComponent<Rigidbody2D>();
            rb.linearVelocity = dir * spellSpeed;
        }
        Debug.Log("Fireball/Iceball được bắn!");
    }

    public void EndAttack()
    {
        isAttacking = false;
        canMove = true;
        cooldownTimer = attackCooldown;
    }

    protected override void Patrol()
    {
        // Boss thường không tuần tra, nhưng nếu muốn có thể thêm:
        if (!canMove) return;

        Vector2 dir = facingLeft ? Vector2.left : Vector2.right;
        transform.Translate(dir * moveSpeed * Time.deltaTime);
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
