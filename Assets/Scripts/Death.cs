using UnityEngine;
using UnityEngine.SceneManagement; // Needed for scene management

public class SceneResetOnTrigger : MonoBehaviour
{
    public string playerTag = "Player"; // Tag to identify the player

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the collider that entered the trigger has the tag of the player
        if (other.CompareTag(playerTag))
        {
            Debug.Log("Player has died! Resetting scene...");
            // Call the method to reset the scene
            ResetScene();
        }
    }

    private void ResetScene()
    {
        // Reload the current scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
