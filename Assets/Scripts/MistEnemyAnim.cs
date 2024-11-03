using UnityEngine;

public class MistEnemyAnim : MonoBehaviour
{
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError("Animator component is missing on the parent object.");
        }
    }

    // Method to trigger attack animation
    public void PlayAttackAnimation()
    {
        if (animator != null)
        {
            animator.SetBool("IsAttacking", true);
            Debug.Log("Attack animation triggered.");
        }
    }

    // Method to reset attack animation
    public void ResetAttackAnimation()
    {
        if (animator != null)
        {
            animator.SetBool("IsAttacking", false);
            Debug.Log("Attack animation reset.");
        }
    }

    // Method to trigger death animation
    public void PlayDeathAnimation()
    {
        if (animator != null)
        {
            animator.SetBool("IsDead", true);
            Debug.Log("Death animation triggered.");
        }
    }
}
