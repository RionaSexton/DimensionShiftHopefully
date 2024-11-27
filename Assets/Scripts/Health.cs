using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class Health : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] private float startingHealth;
    public float currentHealth { get; private set; }

    [Header("iFrames")]
    [SerializeField] private float iFramesDuration;
    [SerializeField] private int numberOfFlashes;
    private SpriteRenderer spriteRend;

    [Header("Animator")]
    [SerializeField] private Animator animator;
    private bool dead;

    private void Awake()
    {
        currentHealth = startingHealth;
        spriteRend = GetComponentInChildren<SpriteRenderer>();

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
            StartCoroutine(Invulnerability());
        }
        else
        {
            if (!dead)
            {
                dead = true;
                HandleDeath();
            }
        }
    }

    public void AddHealth(float _value)
    {
        currentHealth = Mathf.Clamp(currentHealth + _value, 0, startingHealth);
    }

    public void Respawn()
    {
        // Reset death state and restore health
        dead = false;
        AddHealth(startingHealth);

        // Reset Animator state
        if (animator != null)
        {
            animator.ResetTrigger("die");
            animator.Play("Idle2");
        }

        // Reposition the player to the last checkpoint
        Vector3 respawnPosition = CheckpointManager.Instance.GetRespawnPosition();
        transform.position = respawnPosition;

        // Reset Rigidbody state
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = Vector2.zero;
        }

        // Re-enable player movement
        PlayerMovement movement = GetComponent<PlayerMovement>();
        if (movement != null)
        {
            movement.enabled = true;
            movement.ResetMovementState();
        }

        StartCoroutine(Invulnerability());
        Debug.Log("Respawned at: " + transform.position);
    }

    private IEnumerator Invulnerability()
    {
        Physics2D.IgnoreLayerCollision(10, 11, true);

        for (int i = 0; i < numberOfFlashes; i++)
        {
            spriteRend.color = new Color(1, 0, 0, 0.5f);
            yield return new WaitForSeconds(iFramesDuration / (numberOfFlashes * 2));
            spriteRend.color = Color.white;
            yield return new WaitForSeconds(iFramesDuration / (numberOfFlashes * 2));
        }

        Physics2D.IgnoreLayerCollision(10, 11, false);
    }

    private void HandleDeath()
    {
        if (animator != null)
        {
            animator.SetTrigger("die"); // Play death animation
            StartCoroutine(WaitForDeathAnimation());
        }
        else
        {
            Debug.LogWarning("Animator not assigned, respawning immediately.");
            Respawn(); // Respawn immediately if no animator is present
        }
    }

    private IEnumerator WaitForDeathAnimation()
    {
        // Wait for the animation to finish
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);

        Respawn(); // Respawn after the animation
    }
}
