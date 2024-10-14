using System.Collections;
using UnityEngine;

public class DashEnemyMovement : MonoBehaviour
{
    public float moveSpeed = 2f;             // Normal movement speed of the enemy
    public float jumpForce = 5f;             // Jumping force applied to the enemy
    public float chaseDistance = 5f;         // Distance to start chasing the player
    public float attackDistance = 1f;        // Distance to start attacking the player
    public float dashSpeed = 8f;             // Speed during dash
    public float dashDuration = 0.5f;        // Duration of the dash
    public float dashCooldown = 5f;          // Cooldown time between dashes

    public Transform groundCheck;            // A point below the enemy to check for ground
    public Transform wallCheck;              // A point in front of the enemy to check for walls
    public float groundCheckDistance = 0.2f; // Radius to check for ground
    public float wallCheckDistance = 0.2f;   // Distance to check for walls
    public float maxJumpableHeight = 2f;     // Max height the enemy can jump over

    private Rigidbody2D rb;
    private bool isGrounded;
    private Vector2 movementDirection = Vector2.right; // Start moving right

    public Transform player; // Reference to the player's transform
    private bool isChasing = false;
    private bool isDashing = false; // Flag to check if the enemy is currently dashing
    private SpriteRenderer spriteRenderer; // Reference to the SpriteRenderer
    private Health playerHealth; // Reference to the player's Health component

    private Vector2 dashDirection; // Store the direction for the dash
    private float lastDashTime; // Track the time when the last dash occurred

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>(); // Get the SpriteRenderer component
        lastDashTime = -dashCooldown; // Initialize to allow immediate first dash
        playerHealth = player.GetComponent<Health>(); // Get the Health component from the player
    }

    private void Update()
    {
        // Check for wall collision and turn around if necessary
        CheckForWallAndTurn();

        // Check if the enemy is grounded
        isGrounded = IsGrounded();

        // Check distance to the player
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer < chaseDistance && distanceToPlayer > attackDistance)
        {
            // Start chasing the player
            isChasing = true;
            ChasePlayer();
        }
        else if (distanceToPlayer <= attackDistance && !isDashing && Time.time >= lastDashTime + dashCooldown)
        {
            // Start the dash attack
            StartCoroutine(PrepareDashAttack());
        }
        else
        {
            // Resume normal movement
            isChasing = false;
            Wander();
        }
    }

    private bool IsGrounded()
    {
        // Check if the enemy is grounded
        bool grounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckDistance, LayerMask.GetMask("Ground"));
        
        return grounded;
    }

    private void CheckForWallAndTurn()
    {
        RaycastHit2D hit = Physics2D.Raycast(wallCheck.position, movementDirection, wallCheckDistance, LayerMask.GetMask("Wall"));

        if (hit.collider != null)
        {
            Debug.Log("Wall hit detected.");

            float wallHeight = hit.collider.bounds.size.y;
            Debug.Log("Wall detected. Height: " + wallHeight);

            if (wallHeight <= maxJumpableHeight)
            {
                // Jump over the wall regardless of grounded state
                Jump();
                Debug.Log("Jump triggered over a short wall.");
            }
            else
            {
                // Turn around if the wall is too high
                movementDirection *= -1; // Reverse direction
                FlipSprite();
                Debug.Log("Wall too high, turning around.");
            }
        }
        
    }




    private void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
    }

    private void Wander()
    {
        if (!isDashing) // Ensure it only moves if not dashing
        {
            rb.velocity = new Vector2(movementDirection.x * moveSpeed, rb.velocity.y);
        }
    }

    private void ChasePlayer()
    {
        // Move towards the player
        if (!isDashing) // Only chase if not dashing
        {
            if (transform.position.x < player.position.x)
            {
                movementDirection = Vector2.right; // Move right
            }
            else
            {
                movementDirection = Vector2.left; // Move left
            }

            rb.velocity = new Vector2(movementDirection.x * moveSpeed, rb.velocity.y);
        }
    }

    private IEnumerator PrepareDashAttack()
    {
        // Stop moving to prepare for attack
        rb.velocity = Vector2.zero; // Ensure the enemy stands still
        isDashing = true; // Set dashing flag

        // Set enemy scale
        transform.localScale = new Vector3(2f, 1.6f, 1f);

        // Flash the alpha of the sprite renderer
        yield return StartCoroutine(FlashSprite());

        // Increase the wait time before dashing
        yield return new WaitForSeconds(0.5f); // Wait time before dashing

        // Store the dash direction based on the last known movement direction
        dashDirection = movementDirection; // Use the current movement direction

        // Start the actual dash
        StartCoroutine(DashAtPlayer());
    }

    private IEnumerator DashAtPlayer()
    {
        // Ensure the enemy is still before dashing
        rb.velocity = Vector2.zero; // Make sure the enemy is still before dashing

        // Set the dash velocity
        float dashTime = 0f;
        while (dashTime < dashDuration)
        {
            rb.velocity = dashDirection * dashSpeed; // Move in the original dash direction
            dashTime += Time.deltaTime; // Increment the dash time
            yield return null; // Wait for the next frame
        }

        // Check if the enemy hit the player during the dash
        if (Vector2.Distance(transform.position, player.position) < attackDistance &&
            Mathf.Abs(transform.position.y - player.position.y) < 1f) // Check y position difference
        {
            TakeDamage(); // Call the TakeDamage method if the player is hit
        }

        rb.velocity = Vector2.zero; // Stop moving after dashing
        isDashing = false; // Reset dashing flag
        lastDashTime = Time.time; // Record the time of this dash

        // Reset the scale back to normal
        transform.localScale = new Vector3(2f, 2f, 1f);

        // Stay frozen for 1 second after the dash
        yield return new WaitForSeconds(1f); // Freeze for 1 second

        // Resume normal wandering
        Wander(); // Resume wandering behavior
    }

    private void TakeDamage()
    {
        playerHealth.TakeDamage(1); // Inflict 1 damage to the player
        Debug.Log("Player hit! Inflicted 1 damage."); // Debug message to confirm damage
    }

    private IEnumerator FlashSprite()
    {
        // Flash the sprite's alpha from 180 to 255 twice
        Color originalColor = spriteRenderer.color;

        for (int i = 0; i < 2; i++)
        {
            // Flash to semi-transparent
            Color flashColor = new Color(originalColor.r, originalColor.g, originalColor.b, 180f / 255f);
            spriteRenderer.color = flashColor;
            yield return new WaitForSeconds(0.1f);

            // Flash back to original color
            spriteRenderer.color = originalColor;
            yield return new WaitForSeconds(0.1f);
        }
    }

    private void FlipSprite()
    {
        // Flip the sprite to face the movement direction
        Vector3 theScale = transform.localScale;
        theScale.x *= -1; // Flip the x scale
        transform.localScale = theScale;
    }

    private void OnDrawGizmos()
    {
        // Visualize the ground and wall checks
        if (groundCheck != null)
        {
            Gizmos.color = Color.green; // Color for ground check
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckDistance);
        }

        if (wallCheck != null)
        {
            Gizmos.color = Color.red; // Color for wall check
            Gizmos.DrawLine(wallCheck.position, wallCheck.position + (Vector3)movementDirection * wallCheckDistance);
        }
    }
}
