using UnityEngine;
using System.Collections;

public class PlayerProjectile : MonoBehaviour
{
    public float lifetime = 0.5f; // Time in seconds before the projectile is destroyed
    public float destroyDelay = 0.6f; // Delay to wait before destroying gameobject

    private void Start()
    {
        // Destroy the projectile after the specified lifetime
        Destroy(gameObject, lifetime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the projectile hits an enemy
        if (other.CompareTag("Enemy"))
        {
            Animator enemyAnimator = other.GetComponent<Animator>();
            if(enemyAnimator != null)
            {
                // Trigger the death animation
                enemyAnimator.SetTrigger("Die");
                // Destroy the enemy after the animation delay
                Destroy(other.gameObject, destroyDelay);
            }
            Destroy(gameObject); // Destroy the projectile
        }

        // Check if the projectile hits the ground
        if (other.CompareTag("Ground"))
        {
            Destroy(gameObject); // Destroy the projectile
        }
    }

    private void DisableEnemy(GameObject enemy)
    {
        // Disable enemy's collider and movement
        Collider2D enemyCollider = enemy.GetComponent<Collider2D>();
        if (enemyCollider != null)
        {
            enemyCollider.enabled = false;
        }

        DashEnemyMovement enemyMovement = enemy.GetComponent<DashEnemyMovement>();
        if (enemyMovement != null)
        {
            enemyMovement.enabled = false;
        }
    }
}
