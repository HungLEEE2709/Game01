using UnityEngine;

public class PlayerBlock : MonoBehaviour
{
    private Animator animator;
    private Rigidbody2D rb;

    private bool isBlocking = false;
    public bool IsBlocking => isBlocking;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    public void HandleBlock()
    {
        if (Input.GetMouseButtonDown(1))
        {
            animator.SetBool("Idle", false);
            animator.SetBool("IdleBlock", true);
            isBlocking = true;
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
        }

        if (Input.GetMouseButtonUp(1))
        {
            animator.SetBool("Idle", true);
            animator.SetBool("IdleBlock", false);
            isBlocking = false;
        }
    }
}
