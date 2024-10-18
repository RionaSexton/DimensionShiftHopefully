using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneResetOnTrigger : MonoBehaviour
{
    public string playerTag = "Player"; // Tag to identify the player
    public Transform spriteTransform; // Reference to the player's sprite
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
        StartCoroutine(ResetSceneAfterDelay(0.5f)); // 0.5 seconds delay after animation ends
    }

    private IEnumerator ResetSceneAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay); // Wait for the specified delay
        ResetScene(); // Reset the scene
    }

    private void ResetScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
