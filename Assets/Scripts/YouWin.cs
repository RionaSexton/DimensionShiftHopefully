using UnityEngine;

public class TriggerEventWithMusicAndFreeze : MonoBehaviour
{
    public AudioSource backgroundMusic;  // The background music AudioSource
    public AudioSource triumphantMusic;  // The triumphant music AudioSource
    public GameObject player;            // Reference to the player GameObject

    private bool isTriggered = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the object entering the trigger is the player
        if (other.CompareTag("Player") && !isTriggered)
        {
            isTriggered = true;  // Ensure this event only happens once

            // Disable the background music
            if (backgroundMusic != null)
            {
                backgroundMusic.Stop();
            }

            // Play the triumphant music
            if (triumphantMusic != null)
            {
                triumphantMusic.Play();
            }

            // Freeze the entire game by setting time scale to 0
            Time.timeScale = 0f;

            // Optionally, disable player movement (if Time.timeScale = 0 does not stop everything)
            if (player != null)
            {
                var playerMovementScript = player.GetComponent<PlayerMovement>();
                if (playerMovementScript != null)
                {
                    playerMovementScript.enabled = false;
                }
            }
        }
    }
}
