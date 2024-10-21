using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;              // Player walking speed
    public float runSpeed = 8f;               // Player running speed
    public float jumpForce = 10f;             // Jumping force applied to the player
    public Transform spriteTransform;         // Reference to the sprite (child of the player)
    public Transform groundCheck;             // A point below the player to check for ground
    public float groundCheckRadius = 0.2f;    // Radius to check for ground
    private bool isJumping = false;           // For jump animation

    private Rigidbody2D rb;
    private Animator anim;                    // Animator component reference
    private bool isGrounded;
    private bool isDucking = false;           // To track if the player is ducking

    public CapsuleCollider2D standingCollider;    // Reference to the capsule collider used when standing
    public CircleCollider2D crouchingCollider;    // Reference to the circle collider used when crouching

    public AudioClip footstepSound;           // Footstep sound clip
    private AudioSource footstepAudioSource;          // Reference to the AudioSource component

    private float footstepCooldown = 0.5f;    // Time between footsteps
    private float lastFootstepTime = 0f;      // Time when the last footstep sound was played

    // Acceleration variables
    public float acceleration = 10f;          // Acceleration rate
    public float deceleration = 15f;          // Deceleration rate
    private float currentSpeed = 0f;          // The player's current horizontal speed

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = spriteTransform.GetComponent<Animator>(); // Get the Animator component

        // Assign or create a dedicated footstep audio source
        footstepAudioSource = gameObject.AddComponent<AudioSource>(); // Add new AudioSource
        footstepAudioSource.clip = footstepSound;
        footstepAudioSource.loop = false;     // Ensure it doesn't loop

        // Ensure the crouching collider starts off disabled
        crouchingCollider.enabled = false;
    }

    void Update()
    {
        // Handle player movement input (A = left, D = right)
        float moveDirection = Input.GetAxisRaw("Horizontal");

        // Check if Left Shift is being pressed for running
        bool isRunning = Input.GetKey(KeyCode.LeftShift);  // Check if player is running
        float targetSpeed = isRunning ? runSpeed : moveSpeed;

        // Gradually increase or decrease speed based on movement input
        if (Mathf.Abs(moveDirection) > 0.1f)
        {
            currentSpeed = Mathf.MoveTowards(currentSpeed, targetSpeed, acceleration * Time.deltaTime);
        }
        else
        {
            currentSpeed = Mathf.MoveTowards(currentSpeed, 0f, deceleration * Time.deltaTime);
        }

        // Apply movement with acceleration
        rb.velocity = new Vector2(moveDirection * currentSpeed, rb.velocity.y);

        // Check if the player is grounded using a ground check point
        isGrounded = IsGrounded();

        // Set running animation parameter
        anim.SetBool("isRunning", isRunning && moveDirection != 0);

        // Play footstep sound if the player is moving & grounded
        if (isGrounded && Mathf.Abs(moveDirection) > 0.1f)
        {
            PlayFootstepSound(isRunning);
        }
        else
        {
            // Stop the footstep sound if the player stops moving
            if (footstepAudioSource.isPlaying)
            {
                footstepAudioSource.Stop();
            }
        }

        // Jumping mechanic (W or Space to jump)
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            Jump();
        }

        // Check player's velocity so jump anim plays in full
        if (rb.velocity.y > 0.1f || rb.velocity.y < -0.1f)
        {
            isJumping = true;
        }
        else if (isGrounded)
        {
            isJumping = false;
        }

        // Ducking logic using Left Ctrl
        if (Input.GetKey(KeyCode.LeftControl) && !isDucking)
        {
            Duck();  // Call Duck() only once when Left Control is first pressed
        }
        else if (!Input.GetKey(KeyCode.LeftControl) && isDucking)
        {
            StandUp();  // Call StandUp() only once when Left Control is released
        }

        // Flip the sprite based on movement direction and crouch state
        spriteTransform.GetComponent<PlayerSpriteFlip>().FlipSprite(moveDirection, isDucking, isJumping);
    }

    private bool IsGrounded()
    {
        // Check all colliders within the ground check radius
        Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheck.position, groundCheckRadius);
        foreach (Collider2D collider in colliders)
        {
            // If any collider has the tag "Ground", the player is grounded
            if (collider.CompareTag("Ground"))
            {
                isJumping = false; // Reset isJumping when the player is grounded
                return true;
            }
        }
        return false;
    }

    private void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);  // Use velocity instead of linearVelocity
        isJumping = true; // Animation plays
        anim.SetTrigger("jumpTrigger");
    }

    private void Duck()
    {
        isDucking = true;

        // Disable the standing capsule collider and enable the crouching circle collider
        standingCollider.enabled = false;
        crouchingCollider.enabled = true;

        // Set the Animator's isDucking bool to true to trigger the duck animation
        anim.SetBool("isDucking", true);
    }

    private void StandUp()
    {
        isDucking = false;

        // Re-enable the standing capsule collider and disable the crouching circle collider
        standingCollider.enabled = true;
        crouchingCollider.enabled = false;

        // Set the Animator's isDucking bool to false to stop the duck animation
        anim.SetBool("isDucking", false);
    }

    // Play footstep sounds when the player is moving
    private void PlayFootstepSound(bool isRunning)
    {
        // Adjust the cooldown based on walking or running
        float currentCooldown = isRunning ? footstepCooldown / 1.5f : footstepCooldown;

        if (Time.time >= lastFootstepTime + currentCooldown && footstepAudioSource != null && footstepSound != null)
        {
            // Check if the audio is not playing, then play it
            if (!footstepAudioSource.isPlaying)
            {
                footstepAudioSource.Play();
            }

            lastFootstepTime = Time.time; // Update the last footstep time
        }
    }

    // Optional: Visualize the ground check in the editor (helps with debugging)
    private void OnDrawGizmos()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }
}
