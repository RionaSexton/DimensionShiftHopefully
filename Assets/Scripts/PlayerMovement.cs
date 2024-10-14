using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;              // Player walking speed
    public float runSpeed = 8f;               // Player running speed
    public float jumpForce = 10f;             // Jumping force applied to the player
    public Transform spriteTransform;          // Reference to the sprite (child of the player)
    public Transform groundCheck;              // A point below the player to check for ground
    public float groundCheckRadius = 0.2f;     // Radius to check for ground

    private Rigidbody2D rb;
    private CapsuleCollider2D playerCollider;  // Reference to the CapsuleCollider2D for ducking
    private bool isGrounded;
    private bool isDucking = false;            // To track if the player is ducking

    private Vector2 originalColliderSize;      // Original size of the player's capsule collider
    private Vector2 originalColliderOffset;    // Original offset of the player's capsule collider
    private Vector3 originalSpriteScale;       // Original scale of the player's sprite

    private const float crouchHeightMultiplier = 2f / 3f; // Crouching reduces height to 2/3

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<CapsuleCollider2D>();

        // Store the original size and offset of the collider and the sprite scale
        originalColliderSize = playerCollider.size;
        originalColliderOffset = playerCollider.offset;
        originalSpriteScale = spriteTransform.localScale;
    }

    void Update()
    {
        // Handle player movement input (A = left, D = right)
        float moveDirection = Input.GetAxisRaw("Horizontal");

        // Check if Left Shift is being pressed for running
        float currentSpeed = Input.GetKey(KeyCode.LeftShift) ? runSpeed : moveSpeed;

        // Apply movement with the selected speed
        rb.velocity = new Vector2(moveDirection * currentSpeed, rb.velocity.y);  // Use velocity instead of linearVelocity

        // Check if the player is grounded using a ground check point
        isGrounded = IsGrounded();

        // Jumping mechanic (W or Space to jump)
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            Jump();
        }

        // Ducking logic using Left Ctrl
        if (Input.GetKey(KeyCode.LeftControl) && !isDucking)
        {
            Duck();
        }
        else if (Input.GetKeyUp(KeyCode.LeftControl) && isDucking)
        {
            StandUp();
        }

        // Flip the sprite based on movement direction and crouch state
        spriteTransform.GetComponent<PlayerSpriteFlip>().FlipSprite(moveDirection, isDucking);
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
                return true;
            }
        }
        return false;
    }

    private void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);  // Use velocity instead of linearVelocity
    }

    private void Duck()
    {
        isDucking = true;

        // Shrink the player's capsule collider to 2/3 of its original height
        playerCollider.size = new Vector2(originalColliderSize.x, originalColliderSize.y * crouchHeightMultiplier);
        playerCollider.offset = new Vector2(originalColliderOffset.x, originalColliderOffset.y * crouchHeightMultiplier);

        // Shrink the player's sprite to 2/3 of its original height
        spriteTransform.localScale = new Vector3(originalSpriteScale.x, originalSpriteScale.y * crouchHeightMultiplier, originalSpriteScale.z);
    }

    private void StandUp()
    {
        isDucking = false;

        // Restore the player's capsule collider size
        playerCollider.size = originalColliderSize;
        playerCollider.offset = originalColliderOffset;

        // Restore the player's sprite size
        spriteTransform.localScale = originalSpriteScale;
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
