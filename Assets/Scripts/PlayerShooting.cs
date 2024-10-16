using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    public GameObject projectilePrefab; // Reference to the projectile prefab
    public Transform shootPoint;        // Point from where the projectile will be shot
    public float projectileSpeed = 10f; // Speed of the projectile
    public Transform spriteTransform;    // Reference to the sprite to determine its facing direction
    private Animator animator;           // Reference to the Animator for animations

    public float shootCooldown = 1f;    // Time in seconds between shots
    private float lastShootTime = 0f;   // Time when the last shot was fired

    private void Start()
    {
        // Find the Animator component on the child GameObject
        animator = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        // Check for left mouse button click to shoot
        if (Input.GetMouseButtonDown(0) && Time.time >= lastShootTime + shootCooldown) // Left click
        {
            Shoot();
            lastShootTime = Time.time; // Update the last shoot time
        }
    }

    private void Shoot()
    {
        if (projectilePrefab != null && shootPoint != null && spriteTransform != null)
        {
            // Trigger the attack animation
            if (animator != null)
            {
                animator.SetBool("isAttacking", true);
            }

            // Instantiate the projectile at the shoot point
            GameObject projectile = Instantiate(projectilePrefab, shootPoint.position, Quaternion.identity);
            Rigidbody2D projectileRb = projectile.GetComponent<Rigidbody2D>();

            if (projectileRb == null)
            {
                Debug.LogError("Projectile does not have a Rigidbody2D component attached.");
                return; // Exit if there is no Rigidbody2D
            }

            // Determine the direction based on the sprite's current scale
            float direction = spriteTransform.localScale.x > 0 ? 1f : -1f; // Check the x scale for direction
            projectileRb.velocity = new Vector2(direction * projectileSpeed, 0); // Set the projectile's velocity

            // Optional: Call a coroutine to reset the attack animation state after a delay
            StartCoroutine(ResetAttackAnimation());
        }
        else
        {
            if (projectilePrefab == null)
                Debug.LogError("Projectile prefab is not assigned.");
            if (shootPoint == null)
                Debug.LogError("Shoot point is not assigned.");
            if (spriteTransform == null)
                Debug.LogError("Sprite transform is not assigned.");
        }
    }

    private System.Collections.IEnumerator ResetAttackAnimation()
    {
        // Wait for the duration of the attack animation (adjust as needed)
        // You might want to replace this with the actual length of the animation
        yield return new WaitForSeconds(0.5f);

        if (animator != null)
        {
            animator.SetBool("isAttacking", false);
        }
    }
}
