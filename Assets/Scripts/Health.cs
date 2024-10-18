using UnityEngine;
using UnityEngine.SceneManagement; // Add this to access scene management
using System.Collections; // For IEnumerator

public class Health : MonoBehaviour
{
    // Headers just making everything look neater
    [Header("Health")]
    [SerializeField] private float startingHealth; // Set starting health
    public float currentHealth { get; private set; }

    [Header("iFrames")]
    [SerializeField] private float iFramesDuration; // How long iFrames lasts
    [SerializeField] private int numberOfFlashes; // How many colour flashes of the player
    private SpriteRenderer spriteRend;

    [Header("Animator")]
    [SerializeField] private Animator animator; // Reference to the Animator component
    private bool dead;

    private void Awake()
    {
        currentHealth = startingHealth;
        spriteRend = GetComponentInChildren<SpriteRenderer>();

        // Ensure the Animator component is referenced
        if (animator == null)
        {
            Debug.LogWarning("Animator not assigned in Health script on " + gameObject.name);
        }
    }

    public void TakeDamage(float _damage)
    {
        currentHealth = Mathf.Clamp(currentHealth - _damage, 0, startingHealth);

        if (currentHealth > 0)
        {
            // Player hurt (you can add visual/audio feedback here)
            StartCoroutine(Invulnerability());
        }
        else
        {
            if(!dead)
            {
                animator.SetTrigger("die");
                GetComponent<PlayerMovement>().enabled = false;
                dead = true;
                HandleDeath();
            }
            
        }
    }

    private IEnumerator Invulnerability()
    {
        // Create new layers for "Player" and "Enemy"
        // Change 10 to whatever layer player is on (Make its own layer) and 11 whatever layer enemy is on (Also make own layer)
        Physics2D.IgnoreLayerCollision(10, 11, true);

        // Invulnerability duration
        for (int i = 0; i < numberOfFlashes; i++)
        {
            spriteRend.color = new Color(1, 0, 0, 0.5f); // Change RGB on player to 0-1 instead of 0-255
            // iFrames last 2 secs, flash will be 3 in those 2 secs, times 2 because it needs to be 6 flashes in 2 secs because it goes back to white
            yield return new WaitForSeconds(iFramesDuration / (numberOfFlashes * 2));
            spriteRend.color = Color.white;
            yield return new WaitForSeconds(iFramesDuration / (numberOfFlashes * 2));
        }

        Physics2D.IgnoreLayerCollision(10, 11, false); // Change numbers here too
    }

    private void HandleDeath()
    {
        // Start the coroutine to reset the scene after a delay
        StartCoroutine(ResetSceneAfterDelay(1f)); // 1 second delay
    }

    private IEnumerator ResetSceneAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay); // Wait for the specified delay
        ResetScene(); // Reset the scene
    }

    private void ResetScene()
    {
        // Logic to reset the scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
