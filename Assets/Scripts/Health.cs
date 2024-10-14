using UnityEngine;
using UnityEngine.SceneManagement; // Add this to access scene management

public class Health : MonoBehaviour
{
    [SerializeField] private float startingHealth; 
    public float currentHealth { get; private set; }

    private void Awake()
    {
        currentHealth = startingHealth;
    }

    public void TakeDamage(float _damage)
    {
        currentHealth = Mathf.Clamp(currentHealth - _damage, 0, startingHealth);

        if (currentHealth > 0)
        {
            // Player hurt (you can add visual/audio feedback here)
        }
        else
        {
            // Player dead
            ResetScene(); // Reset the scene when health reaches 0
        }
    }

    private void Update()
    {
        // Check if the left mouse button is clicked
        if (Input.GetMouseButtonDown(0)) // 0 is the left mouse button
        {
            TakeDamage(1); // Deal 1 damage
        }
    }

    private void ResetScene()
    {
        // Logic to reset the scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
