using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 5f;           // Speed of the projectile
    private Vector2 direction;         // Direction in which the projectile will move
    public float lifetime = 5f;        // Time in seconds before the projectile is destroyed

    private void Start()
    {
        // Destroy the projectile after 'lifetime' seconds
        Destroy(gameObject, lifetime);
    }

    public void SetDirection(Vector2 newDirection)
    {
        direction = newDirection;
    }

    private void Update()
    {
        // Move the projectile in the set direction
        transform.Translate(direction * speed * Time.deltaTime);
    }

    // Trigger-based collision detection
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the projectile hits the player
        if (collision.CompareTag("Player"))
        {
            // Deal damage to the player
            Health playerHealth = collision.GetComponent<Health>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(1);  // Deal 1 damage to the player
            }

            // Destroy the projectile upon hitting the player
            Destroy(gameObject);
        }

        // Optionally, destroy the projectile when it hits other objects like walls
        if (collision.CompareTag("Ground"))
        {
            Destroy(gameObject);
        }
    }
}
