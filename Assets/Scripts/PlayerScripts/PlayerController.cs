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
    private AudioManager audioManager;

    private bool isRolling = false;
    private float rollTimer = 0f;

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
        audioManager = FindAnyObjectByType<AudioManager>();
    }

    void Update()
    {
        if (playerHealth.IsDead) return;

        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);

        playerBlock.HandleBlock(); 

        if (!playerBlock.IsBlocking && !isRolling && !playerAttack.IsAttacking)
        {
            HandleMovement();
            HandleJump();
            HandleRoll();
        }

        playerAttack.HandleAttack();

        UpdateAnimation();
    }

    private void HandleMovement()
    {
        float moveInput = Input.GetAxisRaw("Horizontal");
        rb.linearVelocity = new Vector2(moveInput * m_Speed, rb.linearVelocity.y);

        if (moveInput != 0)
        {
            transform.localScale = new Vector3(Mathf.Sign(moveInput), 1, 1);
        }
    }

    private void HandleJump()
    {
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            audioManager.PlayJumpSound();
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            animator.SetTrigger("Jump");
        }
    }

    private void HandleRoll()
    {
        if (isRolling)
        {
            rollTimer -= Time.deltaTime;

            float rollDir = Mathf.Sign(transform.localScale.x);
            rb.linearVelocity = new Vector2(rollDir * rollForce, rb.linearVelocity.y);

            if (rollTimer <= 0f)
            {
                isRolling = false; 
            }

            return;
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && isGrounded && !playerAttack.IsAttacking && !playerBlock.IsBlocking)
        {
            isRolling = true;
            rollTimer = rollDuration;
            animator.SetTrigger("Roll");
            float rollDir = Mathf.Sign(transform.localScale.x);
            rb.linearVelocity = new Vector2(rollDir * rollForce, rb.linearVelocity.y);
        }
    }
    public void EndRoll()
    {
        isRolling = false;
        animator.ResetTrigger("Roll");
    }

    private void UpdateAnimation()
    {
        animator.SetBool("Grounded", isGrounded);
        animator.SetFloat("AirSpeed", rb.linearVelocity.y);
        animator.SetBool("isRunning", Mathf.Abs(rb.linearVelocity.x) > 0.1f);

        if (!playerBlock.IsBlocking)
        {
            animator.SetBool("Idle", isGrounded && Mathf.Abs(rb.linearVelocity.x) < 0.1f);
        }
    }
}
