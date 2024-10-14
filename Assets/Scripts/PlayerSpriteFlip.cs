using UnityEngine;

public class PlayerSpriteFlip : MonoBehaviour
{
    

    private void Start()
    {
        
    }

    // Flip the sprite and control animations based on the movement direction
    public void FlipSprite(float moveDirection, bool isDucking)
    {
        // Get current local scale to preserve vertical scaling (for crouching)
        Vector3 currentScale = transform.localScale;

        // Flip the sprite based on movement direction
        if (moveDirection < 0)
        {
            transform.localScale = new Vector3(-Mathf.Abs(currentScale.x), currentScale.y, currentScale.z); // Flip left
        }
        else if (moveDirection > 0)
        {
            transform.localScale = new Vector3(Mathf.Abs(currentScale.x), currentScale.y, currentScale.z);  // Flip right
        }

        // Ensure the scale remains appropriate for crouching (handled in PlayerMovement)
        if (isDucking)
        {
            transform.localScale = new Vector3(transform.localScale.x, currentScale.y, currentScale.z); // Maintain crouch size
        }

    

    }
}
