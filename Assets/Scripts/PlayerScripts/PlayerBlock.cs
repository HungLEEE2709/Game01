using UnityEngine;

public class PlayerBlock : MonoBehaviour
{
    private Animator animator;
    private Rigidbody2D rb;
    private PlayerHealth playerHealth;

    private bool isBlocking = false;
    public bool IsBlocking => isBlocking;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        playerHealth = GetComponent<PlayerHealth>();
    }

    private void Update()
    {
        HandleBlock();
    }

    public void HandleBlock()
    {
        // Nếu giữ chuột phải để block
        if (Input.GetMouseButton(1))
        {
            if (!isBlocking)
            {
                isBlocking = true;
                playerHealth.IsBlocking = true;

                animator.SetBool("Idle", false); // tắt idle gốc
                animator.SetBool("IdleBlock", true); // bật idle block

                rb.linearVelocity = new Vector2(0, rb.linearVelocity.y); // đứng im khi block
                Debug.Log("Bắt đầu block");
            }
        }
        else
        {
            if (isBlocking)
            {
                isBlocking = false;
                playerHealth.IsBlocking = false;

                animator.SetBool("IdleBlock", false);
                animator.SetBool("Idle", true); // quay lại idle

                Debug.Log("Ngừng block");
            }
        }
    }

    // Dùng khi enemy đánh trúng (gọi từ PlayerHealth)
    public void TriggerBlock()
    {
        if (isBlocking)
        {
            animator.SetTrigger("Block");
        }
    }
}
