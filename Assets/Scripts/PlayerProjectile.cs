using UnityEngine;

public class PlayerProjectile : MonoBehaviour
{
    public float lifetime = 0.5f; // Time in seconds before the projectile is destroyed

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
            Destroy(other.gameObject); // Destroy the enemy
            Destroy(gameObject); // Destroy the projectile
        }

        // Check if the projectile hits the ground
        if (other.CompareTag("Ground"))
        {
            Destroy(gameObject); // Destroy the projectile
        }
    }
}
