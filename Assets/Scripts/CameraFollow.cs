using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;
    [SerializeField] private float offsetX = 0f; // Horizontal offset for the camera
    [SerializeField] private float offsetY = 0f; // Vertical offset for the camera (Change both of these to set where you want the player in relation to the camera)
    [SerializeField] private float smoothSpeed = 0.125f; // Smoothness factor for camera movement

    public LayerMask groundLayer; // LayerMask to identify platforms or ground (Set a layer for the ground if haven't already)
    public float groundCheckDistance = 0.1f; // Distance for ground check (used for raycasting)

    private float lastGroundedY; // Last Y position when the player was grounded
    private bool isGrounded; // Whether the player is grounded
    private Rigidbody2D playerRb; // Reference to the player's Rigidbody2D to check velocity

    private float cameraTargetY; // Store the target Y position for the camera

    void Start()
    {
        // Get the player's Rigidbody2D for checking vertical velocity
        playerRb = player.GetComponent<Rigidbody2D>();
        cameraTargetY = transform.position.y; // Initialize targetY
    }

    void LateUpdate()
    {
        // Perform a raycast down from the player's position to check if grounded
        isGrounded = Physics2D.Raycast(player.position, Vector2.down, groundCheckDistance, groundLayer);

        // Calculate the target Y position based on the player's position
        float targetY = player.position.y + offsetY;

        // Only update the camera's Y position if the player is below or above the current camera position
        if (player.position.y < transform.position.y - offsetY) // Player falls below camera
        {
            cameraTargetY = Mathf.Lerp(cameraTargetY, targetY, smoothSpeed);
        }
        else if (player.position.y > transform.position.y + offsetY) // Player rises above camera
        {
            cameraTargetY = Mathf.Lerp(cameraTargetY, targetY, smoothSpeed);
        }

        // Set the desired position for the camera
        Vector3 desiredPosition = new Vector3(player.position.x + offsetX, cameraTargetY, transform.position.z);

        // Smoothly interpolate towards the desired position
        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
    }
}