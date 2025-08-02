using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 12f;
    [SerializeField] private float variableJumpHeightMultiplier = 0.5f;
    [SerializeField] private float coyoteTime = 0.2f;
    [SerializeField] private float jumpBufferTime = 0.2f;

    private Rigidbody2D rb;
    private Animator animator;
    private bool isGrounded;
    private bool wasGroundedLastFrame;
    private float horizontalInput;
    private bool facingRight = true;

    private float coyoteTimeCounter;
    private float jumpBufferCounter;

    private AudioSource audioSource;
    public AudioClip jumpSound;
    public AudioClip footstepSound;
    public AudioClip landingSound;

    private float footstepTimer = 0f;
    [SerializeField] private float footstepInterval = 0.4f;

    public bool canMove = true;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        wasGroundedLastFrame = false;
    }

    private void Update()
    {
        if (!canMove)
        {
            // Disable movement and set gravity scale to 0
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
            rb.gravityScale = 0;
            return;
        }

        horizontalInput = Input.GetAxisRaw("Horizontal");

        // Direct horizontal movement without acceleration/deceleration
        float targetVelocityX = horizontalInput * moveSpeed;
        rb.linearVelocity = new Vector2(targetVelocityX, rb.linearVelocity.y);

        // Footstep sound logic
        if (isGrounded && Mathf.Abs(horizontalInput) > 0.1f)
        {
            footstepTimer -= Time.deltaTime;
            if (footstepTimer <= 0f)
            {
                if (audioSource != null && footstepSound != null)
                {
                    audioSource.PlayOneShot(footstepSound);
                }
                footstepTimer = footstepInterval;
            }
        }
        else
        {
            footstepTimer = 0f;
        }

        // Coyote time counter update
        if (isGrounded)
        {
            coyoteTimeCounter = coyoteTime;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }

        // Jump buffer counter update
        if (Input.GetButtonDown("Jump"))
        {
            jumpBufferCounter = jumpBufferTime;
        }
        else
        {
            jumpBufferCounter -= Time.deltaTime;
        }

        // Jump if within coyote time and jump buffer
        if (coyoteTimeCounter > 0f && jumpBufferCounter > 0f)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            isGrounded = false;
            jumpBufferCounter = 0f;
            if (audioSource != null && jumpSound != null)
            {
                audioSource.PlayOneShot(jumpSound);
            }
        }

        // Variable jump height
        if (rb.linearVelocity.y > 0 && !Input.GetButton("Jump"))
        {
            rb.linearVelocity += Vector2.up * Physics2D.gravity.y * (variableJumpHeightMultiplier - 1) * Time.deltaTime;
        }

        // Update animations
        UpdateAnimations();

        // Flip player sprite based on movement direction
        if (horizontalInput > 0 && !facingRight)
        {
            Flip();
        }
        else if (horizontalInput < 0 && facingRight)
        {
            Flip();
        }

        // Landing sound logic
        if (!wasGroundedLastFrame && isGrounded)
        {
            if (audioSource != null && landingSound != null)
            {
                audioSource.PlayOneShot(landingSound);
            }
        }
        wasGroundedLastFrame = isGrounded;
    }

    public void DisableMovement()
    {
        canMove = false;
        rb.gravityScale = 0;
        rb.linearVelocity = Vector2.zero;
    }

    public void DisableMovement2()
    {
        canMove = false;
        rb.gravityScale = 5;
        rb.linearVelocity = Vector2.zero;
    }
    public void CanMovement()
    {
        canMove = true;
        rb.gravityScale = 4;
    }

    private void UpdateAnimations()
    {
        animator.SetBool("isRun", Mathf.Abs(rb.linearVelocity.x) > 0.1f);
        animator.SetBool("isJump", !isGrounded);
        animator.SetBool("idle", Mathf.Abs(rb.linearVelocity.x) <= 0.1f && isGrounded);
    }

    private void Flip()
    {
        facingRight = !facingRight;
        Vector3 scale = transform.localScale; 
        scale.x *= -1;
        transform.localScale = scale;
    }

    [SerializeField] private Transform groundCheckPoint;
    [SerializeField] private float groundCheckDistance = 0.7f;
    [SerializeField] private LayerMask groundLayer;

    private void FixedUpdate()
    {
        RaycastHit2D hit = Physics2D.Raycast(groundCheckPoint.position, Vector2.down, groundCheckDistance, groundLayer);
        isGrounded = hit.collider != null;
    }
}
