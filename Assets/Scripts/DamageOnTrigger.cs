using UnityEngine;

public class DamageOnTrigger : MonoBehaviour
{
    public float damageAmount = 1f; // Amount of damage to apply
    public string playerTag = "Player"; // Tag for the player to identify the correct GameObject

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the object entering the trigger has the player tag
        if (other.CompareTag(playerTag))
        {
            // Attempt to get the Health component from the player
            Health playerHealth = other.GetComponent<Health>();
            if (playerHealth != null)
            {
                // Call TakeDamage on the player's Health script
                playerHealth.TakeDamage(damageAmount);
            }
            else
            {
                Debug.LogWarning("No Health component found on the player.");
            }
        }
    }
}

