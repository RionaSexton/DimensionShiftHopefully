using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float runSpeed = 8f;
    public float jumpForce = 10f;
    public Transform spriteTransform;
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    private bool isJumping = false;

    private Rigidbody2D rb;
    private Animator anim;
    private bool isGrounded;
    private bool isDucking = false;

    public CapsuleCollider2D standingCollider;
    public CircleCollider2D crouchingCollider;

    public AudioClip footstepSound;
    private AudioSource footstepAudioSource;

    private float footstepCooldown = 0.5f;
    private float lastFootstepTime = 0f;

    public float acceleration = 10f;
    public float deceleration = 15f;
    private float currentSpeed = 0f;

    public float jumpCooldown = 1.0f;
    private float lastJumpTime = -1.0f;

    private bool canMove = true; // For movement control

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = spriteTransform.GetComponent<Animator>();

        footstepAudioSource = gameObject.AddComponent<AudioSource>();
        footstepAudioSource.clip = footstepSound;
        footstepAudioSource.loop = false;

        crouchingCollider.enabled = false;
    }

    void Update()
    {
        if (!canMove)
        {
            Debug.LogWarning("Player cannot move. Movement is disabled.");
            return; // Prevent further execution
        }

        float moveDirection = Input.GetAxisRaw("Horizontal");

        if (Mathf.Abs(moveDirection) > 0.1f)
        {
            Debug.Log("Player is moving. Input detected: " + moveDirection);
        }

        bool isRunning = Input.GetKey(KeyCode.LeftShift);
        float targetSpeed = isRunning ? runSpeed : moveSpeed;

        if (Mathf.Abs(moveDirection) > 0.1f)
        {
            currentSpeed = Mathf.MoveTowards(currentSpeed, targetSpeed, acceleration * Time.deltaTime);
        }
        else
        {
            currentSpeed = Mathf.MoveTowards(currentSpeed, 0f, deceleration * Time.deltaTime);
        }

        rb.velocity = new Vector2(moveDirection * currentSpeed, rb.velocity.y);

        isGrounded = IsGrounded();

        anim.SetBool("isRunning", isRunning && moveDirection != 0);

        if (isGrounded && Mathf.Abs(moveDirection) > 0.1f)
        {
            PlayFootstepSound(isRunning);
        }
        else
        {
            if (footstepAudioSource.isPlaying)
            {
                footstepAudioSource.Stop();
            }
        }

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            if (Time.time >= lastJumpTime + jumpCooldown)
            {
                Jump();
                lastJumpTime = Time.time;
            }
        }

        if (rb.velocity.y > 0.1f || rb.velocity.y < -0.1f)
        {
            isJumping = true;
        }
        else if (isGrounded)
        {
            isJumping = false;
        }

        if (Input.GetKey(KeyCode.LeftControl) && !isDucking)
        {
            Duck();
        }
        else if (!Input.GetKey(KeyCode.LeftControl) && isDucking)
        {
            StandUp();
        }

        spriteTransform.GetComponent<PlayerSpriteFlip>().FlipSprite(moveDirection, isDucking, isJumping);
    }

    private bool IsGrounded()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheck.position, groundCheckRadius);
        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag("Ground"))
            {
                isJumping = false;
                return true;
            }
        }
        return false;
    }

    private void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        isJumping = true;
        anim.SetTrigger("jumpTrigger");
    }

    private void Duck()
    {
        isDucking = true;
        standingCollider.enabled = false;
        crouchingCollider.enabled = true;
        anim.SetBool("isDucking", true);
    }

    private void StandUp()
    {
        isDucking = false;
        standingCollider.enabled = true;
        crouchingCollider.enabled = false;
        anim.SetBool("isDucking", false);
    }

    private void PlayFootstepSound(bool isRunning)
    {
        float currentCooldown = isRunning ? footstepCooldown / 1.5f : footstepCooldown;

        if (Time.time >= lastFootstepTime + currentCooldown && footstepAudioSource != null && footstepSound != null)
        {
            if (!footstepAudioSource.isPlaying)
            {
                footstepAudioSource.Play();
            }

            lastFootstepTime = Time.time;
        }
    }

    public void ResetMovementState()
    {
        currentSpeed = 0f;
        isDucking = false;
        isJumping = false;
        rb.velocity = Vector2.zero;

        // Reset animator parameters if needed
        anim.SetBool("isRunning", false);
        anim.SetBool("isDucking", false);
        anim.SetTrigger("Idle2");
    }


    private void OnDrawGizmos()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }
}
