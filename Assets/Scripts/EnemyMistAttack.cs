using System.Collections;
using UnityEngine;

public class EnemyMistAttack : MonoBehaviour
{
    public float damageInterval = 2f;    // Time between each damage tick
    public int damageAmount = 1;         // Damage dealt to the player per tick
    private bool playerInMist = false;   // Tracks if the player is in the mist area
    private float nextDamageTime;        // Time to apply the next damage tick
    private GameObject player;           // Reference to the player GameObject

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))  // Check if the player entered the mist area
        {
            playerInMist = true;
            player = other.gameObject; // Store the player reference
            nextDamageTime = Time.time + damageInterval;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))  // Check if the player exited the mist area
        {
            playerInMist = false;
            player = null; // Clear the player reference
        }
    }

    private void Update()
    {
        if (playerInMist && Time.time >= nextDamageTime)
        {
            if(player != null)
            {
                // Apply damage to the player
                player.GetComponent<Health>().TakeDamage(damageAmount);

                // Reset the timer for the next damage tick
                nextDamageTime = Time.time + damageInterval;
            }
        }
    }
}
