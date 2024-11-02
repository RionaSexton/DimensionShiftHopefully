using UnityEngine;

public class EnemyMistAttack : MonoBehaviour
{
    public float damageInterval = 2f;        // Time between each damage tick
    public int damageAmount = 1;             // Damage dealt to the player per tick
    public int enemyHealth = 3;              // Enemy's health (for triggering death)
    
    private bool playerInMist = false;       // Tracks if the player is in the mist area
    private float nextDamageTime;            // Time to apply the next damage tick
    private GameObject player;               // Reference to the player GameObject
    private Animator animator;               // Reference to the Animator component on the parent object

    private void Start()
    {
        animator = GetComponentInParent<Animator>(); // Access Animator component on the parent
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))      // Check if the player entered the mist area
        {
            playerInMist = true;
            player = other.gameObject;       // Store the player reference
            nextDamageTime = Time.time + damageInterval;

            // Trigger the attack animation
            animator.SetBool("IsAttacking", true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))      // Check if the player exited the mist area
        {
            playerInMist = false;
            player = null;                   // Clear the player reference

            // Revert to idle animation
            animator.SetBool("IsAttacking", false);
        }
    }

    private void Update()
    {
        if (playerInMist && Time.time >= nextDamageTime)
        {
            if (player != null)
            {
                // Apply damage to the player
                player.GetComponent<Health>().TakeDamage(damageAmount);

                // Reset the timer for the next damage tick
                nextDamageTime = Time.time + damageInterval;
            }
        }
    }

    // Method to apply damage to the enemy
    public void TakeDamage(int damage)
    {
        enemyHealth -= damage;
        
        if (enemyHealth <= 0)
        {
            Die();
        }
    }

    // Method to handle the death of the enemy
    private void Die()
    {
        // Trigger the die animation
        animator.SetTrigger("Die");

        // Optional: Disable further interaction
        playerInMist = false;
        GetComponent<Collider2D>().enabled = false; // Disable collider to stop any triggers
    }
}
