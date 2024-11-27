using UnityEngine;

public class Death : MonoBehaviour
{
    public string playerTag = "Player"; // Tag to identify the player

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(playerTag))
        {
            Health playerHealth = other.GetComponent<Health>();

            if (playerHealth != null)
            {
                // Set the player's health to 0 and handle respawn
                playerHealth.TakeDamage(playerHealth.currentHealth);
            }
            else
            {
                Debug.LogError("No Health script found on the player.");
            }
        }
    }
}
