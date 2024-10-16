using UnityEngine;

public class Lever : MonoBehaviour
{
    public GameObject objectToActivate; // The game object to enable
    public GameObject objectToDeactivate1; // The first game object to disable
    public GameObject objectToDeactivate2; // The second game object to disable
    public Collider2D triggerCollider; // The Collider2D to check for triggers
    public AudioClip soundClip; // The sound clip to play

    private AudioSource audioSource; // Reference to the AudioSource component

    void Start()
    {
        // Get the AudioSource component from this GameObject
        audioSource = GetComponent<AudioSource>();

        // Check if the AudioSource is missing and log a warning if it is
        if (audioSource == null)
        {
            Debug.LogWarning("AudioSource not found on " + gameObject.name + ". Please add an AudioSource component.");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the object that entered the trigger has the correct tag
        if (other.CompareTag("Player")) // Make sure to change "Player" to your actual player's tag
        {
            Debug.Log("Collision detected with player.");

            // Enable the specified game object
            objectToActivate.SetActive(true);

            // Disable the other two game objects
            objectToDeactivate1.SetActive(false);
            objectToDeactivate2.SetActive(false);

            // Play the sound clip & destroy it so it doesn't replay
            PlaySoundAndDestroy();
        }
    }

    private void PlaySoundAndDestroy()
    {
        if (audioSource != null && soundClip != null)
        {
            audioSource.PlayOneShot(soundClip);
            Destroy(audioSource, soundClip.length);
        }
        else
        {
            Debug.LogWarning("Sound clip not assigned in the inspector or AudioSource is missing.");
        }
    }
}
