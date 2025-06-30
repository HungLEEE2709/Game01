using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float m_Speed = 4.0f;
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float rollForce = 12f;
    [SerializeField] private float rollDuration = 0.5f;

    [Header("Ground Check")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    private bool isGrounded;

    private Rigidbody2D rb;
    private Animator animator;

    private bool isRolling = false;
    private float rollTimer = 0f;

    // Thêm các module con
    private PlayerAttack playerAttack;
    private PlayerBlock playerBlock;
    private PlayerHealth playerHealth;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        playerAttack = GetComponent<PlayerAttack>();
        playerBlock = GetComponent<PlayerBlock>();
        playerHealth = GetComponent<PlayerHealth>();
    }

    void Update()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);

        if (playerHealth.IsDead) return;

        playerBlock.HandleBlock();
        HandleMovement();
        HandleJump();
        HandleRoll();
        playerAttack.HandleAttack();
        UpdateAnimation();
    }

    private void HandleMovement()
    {
        if (isRolling || playerAttack.IsAttacking || playerBlock.IsBlocking || playerHealth.IsDead) return;

        float moveInput = Input.GetAxisRaw("Horizontal");
        rb.linearVelocity = new Vector2(moveInput * m_Speed, rb.linearVelocity.y);

        if (moveInput != 0)
        {
            transform.localScale = new Vector3(Mathf.Sign(moveInput), 1, 1);
        }
    }

    private void HandleJump()
    {
        if (Input.GetButtonDown("Jump") && isGrounded && !isRolling)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            animator.SetTrigger("Jump");
        }
    }

    private void HandleRoll()
    {
        if (isRolling)
        {
            rollTimer -= Time.deltaTime;

            float rollDirection = Mathf.Sign(transform.localScale.x);
            rb.linearVelocity = new Vector2(rollDirection * rollForce, rb.linearVelocity.y);

            if (rollTimer <= 0f)
            {
                isRolling = false;
                animator.SetBool("Roll", false);
                rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
            }
            return;
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && isGrounded && !playerAttack.IsAttacking && !playerHealth.IsDead)
        {
            isRolling = true;
            rollTimer = rollDuration;
            animator.SetBool("Roll", true);
            float rollDirection = Mathf.Sign(transform.localScale.x);
            rb.linearVelocity = new Vector2(rollDirection * rollForce, rb.linearVelocity.y);
        }
    }

    private void UpdateAnimation()
    {
        animator.SetBool("Grounded", isGrounded);
        animator.SetFloat("AirSpeed", rb.linearVelocity.y);
        animator.SetBool("isRunning", Mathf.Abs(rb.linearVelocity.x) > 0.1f);
    }
}
