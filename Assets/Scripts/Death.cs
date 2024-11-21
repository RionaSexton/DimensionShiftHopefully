using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class Death : MonoBehaviour
{
    public string playerTag = "Player"; // Tag to identify the player
    public Transform spriteTransform; // Reference to the player's sprite
    public Transform playerTransform;
    private Animator animator;
    private bool dead;

    private float startingHealth = 5f; // Set starting health
    public float currentHealth { get; private set; }

    private void Awake()
    {
        // Check if spriteTransform is assigned
        if (spriteTransform == null)
        {
            Debug.LogError("spriteTransform is not assigned in the Inspector.");
        }
        else
        {
            // Attempt to get the Animator component
            animator = spriteTransform.GetComponent<Animator>();

            // Check if the animator exists
            if (animator == null)
            {
                Debug.LogError("No Animator component found on spriteTransform!");
            }
        }

        currentHealth = startingHealth; // Initialize health
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(playerTag) && !dead)
        {
            TakeDamage(startingHealth); // Take full damage (death)
        }
    }

    public void TakeDamage(float _damage)
    {
        currentHealth = Mathf.Clamp(currentHealth - _damage, 0, startingHealth);

        if (currentHealth <= 0 && !dead)
        {
            dead = true;

            // Trigger death animation if animator exists
            if (animator != null)
            {
                animator.SetTrigger("die");
            }
            else
            {
                Debug.LogWarning("Cannot play death animation because animator is null.");
            }

            StartCoroutine(WaitForDeathAnimation()); // Wait for animation
        }
    }

    private IEnumerator WaitForDeathAnimation()
    {
        // Hardcoded wait to simulate waiting for animation duration
        yield return new WaitForSeconds(1f);
        HandleDeath(); // Handle the death (reset scene)
    }

    private void HandleDeath()
    {
        // Check if CheckpointManager has been set and if there is a valid checkpoint
        if (CheckpointManager.Instance != null)
        {
            Vector3 respawnPosition = CheckpointManager.Instance.GetRespawnPosition();

            if (respawnPosition == new Vector3(0, 0, 0)) // No checkpoint set
            {
                Debug.Log("No checkpoint set. Fully resetting the scene.");
                // Call ForceReloadScene when no checkpoint is available
                StartCoroutine(ForceReloadScene());
            }
            else
            {
                // Respawn the player at the last checkpoint if one is set
                if (playerTransform != null)
                {
                    playerTransform.position = respawnPosition;
                    Debug.Log("Player moved to respawn position: " + playerTransform.position);

                    ResetPlayerHealth();
                    dead = false;

                    if (animator != null)
                    {
                        animator.ResetTrigger("die");
                        animator.Play("Idle2");
                    }

                    GetComponent<PlayerMovement>().enabled = true; // Enable movement again
                }
                else
                {
                    Debug.LogError("Player Transform is not assigned in the Death script.");
                }
            }
        }
        else
        {
            // If the CheckpointManager is not available, fully reset the scene
            Debug.LogError("CheckpointManager.Instance is null! Fully resetting the scene.");
            StartCoroutine(ForceReloadScene());
        }
    }

    private void ResetPlayerHealth()
{
    Health healthScript = GetComponent<Health>();
    if (healthScript != null)
    {
        healthScript.Respawn();  // Ensure health is reset when respawning
    }
    else
    {
        Debug.LogError("Health script is missing on the player.");
    }
}

    private IEnumerator ForceReloadScene()
    {
        string sceneName = SceneManager.GetActiveScene().name;

        // Unload the current scene
        yield return SceneManager.UnloadSceneAsync(sceneName);

        // Reload the scene
        yield return SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);

        Debug.Log("Scene fully reloaded.");
    }
}
