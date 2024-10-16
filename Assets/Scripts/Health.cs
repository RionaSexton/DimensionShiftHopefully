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

    private void Awake()
    {
        currentHealth = startingHealth;
        spriteRend = GetComponentInChildren<SpriteRenderer>();
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
            // Player dead
            ResetScene(); // Reset the scene when health reaches 0
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

    private void ResetScene()
    {
        // Logic to reset the scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}