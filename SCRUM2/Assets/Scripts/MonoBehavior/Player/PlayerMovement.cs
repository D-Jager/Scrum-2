using UnityEngine;
using System;

public class PlayerMovement : MonoBehaviour
{
    [Header("Wants")]
     public float wantMove;
     public bool wantJump = false;
     public bool wantRun = false;

    [Header("Stats")]
     public float speed = 5f;
     public float runMult { get; private set; } = 1.5f;
     public float jumpPow = 8f;

    [Header("State")]
     public bool isGrounded = false;
     public bool jumpSlow = false;
     public bool isDead = false;

    [Header("Permissions")]
     public bool canMove = true;
     public bool canJump = true;

    [Header("Components")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private Animator animator;

    [Header("Ground check")]
    [SerializeField] private Transform GroundCheck;
    [SerializeField] private LayerMask groundLayer;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        wantMove = Input.GetAxisRaw("Horizontal");
        wantJump = Input.GetButton("Jump");
        wantRun = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);

        UpdateSRFlip();
        UpdateAnimator();
    }

    private void FixedUpdate()
    {
        if (canMove && !isDead) rb.linearVelocity = new Vector2(wantMove * speed * (wantRun ? runMult : 1f), rb.linearVelocity.y);

        isGrounded = CheckGrounded();

        if (canJump && !isDead)
        {
            if (wantJump && isGrounded)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpPow);
                jumpSlow = false;
            }

            if (!wantJump && rb.linearVelocity.y > 0f && !isGrounded && !jumpSlow)
            {
                rb.linearVelocity *= new Vector2(1, 0.5f);
                jumpSlow = true;
            }
        }
    }

    public bool CheckGrounded()
    {
        return Physics2D.OverlapCircle(GroundCheck.position, 0.01f, groundLayer);
    }

    public void UpdateAnimator()
    {
        if (!animator) return;

        animator.SetFloat("wantMove", Math.Abs(wantMove));
        animator.SetFloat("verticalSpeed", rb.linearVelocityY);
        animator.SetBool("isGrounded", isGrounded);
        animator.SetBool("wantRun", wantRun);
        animator.SetBool("isDead", isDead);
    }

    public void UpdateSRFlip()
    {
        if (isDead) return;

        if (!sr.flipX && wantMove < 0f) sr.flipX = true;
        if (sr.flipX && wantMove > 0f) sr.flipX = false;
    }
}