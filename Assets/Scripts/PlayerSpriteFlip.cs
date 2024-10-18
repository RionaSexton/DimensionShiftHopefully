using UnityEngine;

public class PlayerSpriteFlip : MonoBehaviour
{
    private Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Flip the sprite and control animations based on the movement direction
    public void FlipSprite(float moveDirection, bool isDucking, bool isJumping)
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

        // Set the isDucking parameter in the Animator, playing the crouch/duck animation
        anim.SetBool("isDucking", isDucking);

        // Control animations based on movement direction
        anim.SetFloat("Speed", Mathf.Abs(moveDirection)); // Set the Speed parameter for the walking animation

        if (moveDirection == 0)
        {
            anim.SetBool("isIdle", true);  // Set the Idle animation when not moving
        }
        else
        {
            anim.SetBool("isIdle", false); // Disable the Idle animation when moving
        }

        // Control Jump animation
        anim.SetBool("isJumping", isJumping);
    }
}
