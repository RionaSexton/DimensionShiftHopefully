using System.Collections;
using UnityEngine;

public class ProjectileEnemyMovement : MonoBehaviour
{
    public float moveSpeed = 2f;              // Normal movement speed of the enemy
    public float jumpForce = 5f;              // Jumping force applied to the enemy
    public float chaseDistance = 5f;          // Distance to start chasing the player
    public float attackDistance = 1f;         // Distance to start attacking the player
    public float projectileCooldown = 2f;     // Cooldown time between projectile attacks
    public GameObject projectilePrefab;        // Projectile prefab to shoot
    public Transform projectileSpawnPoint;     // Point where the projectile will be spawned
    public Transform groundCheck;              // A point below the enemy to check for ground
    public Transform wallCheck;                // A point in front of the enemy to check for walls
    public float groundCheckDistance = 0.2f;  // Radius to check for ground
    public float wallCheckDistance = 0.2f;    // Distance to check for walls
    public float maxJumpableHeight = 2f;      // Max height the enemy can jump over

    private Rigidbody2D rb;
    private bool isGrounded;
    private Vector2 movementDirection = Vector2.right; // Start moving right
    public Transform player; // Reference to the player's transform
    private bool isChasing = false;
    private SpriteRenderer spriteRenderer; // Reference to the SpriteRenderer
    private Health playerHealth; // Reference to the player's Health component
    private float lastProjectileTime; // Track the time when the last projectile was fired

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>(); // Get the SpriteRenderer component
        lastProjectileTime = -projectileCooldown; // Initialize to allow immediate first shot
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

        if (distanceToPlayer < chaseDistance)
        {
            // Start chasing the player
            isChasing = true;
            ChasePlayer();

            if (distanceToPlayer <= attackDistance && Time.time >= lastProjectileTime + projectileCooldown)
            {
                // Start shooting a projectile
                ShootProjectile();
            }
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
        Debug.Log("Is Grounded: " + grounded);
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
        rb.velocity = new Vector2(movementDirection.x * moveSpeed, rb.velocity.y);
    }

    private void ChasePlayer()
    {
        // Move towards the player
        if (!isChasing) // Only chase if not dashing
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

    private void ShootProjectile()
    {
        // Calculate the direction to the player
        Vector2 directionToPlayer = (player.position - projectileSpawnPoint.position).normalized;

        // Create a new projectile instance
        GameObject projectile = Instantiate(projectilePrefab, projectileSpawnPoint.position, Quaternion.identity);

        // Pass the calculated direction to the projectile
        Projectile projectileScript = projectile.GetComponent<Projectile>();
        projectileScript.SetDirection(directionToPlayer);

        lastProjectileTime = Time.time; // Record the time of this projectile shot
        Debug.Log("Projectile shot at player!");
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
