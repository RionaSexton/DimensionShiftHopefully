using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class TriggerEventWithMusicAndFreeze : MonoBehaviour
{
    public AudioSource backgroundMusic;  // The background music AudioSource
    public AudioSource triumphantMusic;  // The triumphant music AudioSource
    public GameObject player;            // Reference to the player GameObject
    public float delayBeforeNextLevel = 2.5f;  // Time in seconds before moving to the next level

    private bool isTriggered = false;

    private void NextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

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

            StartCoroutine(DelayedNextLevel());
        }
    }

    private IEnumerator DelayedNextLevel()
    {
        yield return new WaitForSeconds(delayBeforeNextLevel);
        NextLevel();
    }
}
