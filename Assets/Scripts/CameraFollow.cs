using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;   // Reference to the player's transform

    [Header("Stabilization Settings")]
    public bool stabilizeX = true;  // Toggle stabilization for the X axis
    public bool stabilizeY = true;  // Toggle stabilization for the Y axis
    public float stabilizationSpeedX = 0.2f;  // Speed of stabilization for the X axis (damping time)
    public float stabilizationSpeedY = 0.2f;  // Speed of stabilization for the Y axis (damping time)
    public float maxCameraSpeed = 15f;         // Maximum speed the camera can move to catch up

    private Vector3 offset;  // The initial offset between the camera and the player
    private float velocityX = 0.0f; // Used to smoothly damp X axis
    private float velocityY = 0.0f; // Used to smoothly damp Y axis

    void Start()
    {
        // Calculate and store the initial offset between the player and camera position
        if (player != null)
        {
            offset = transform.position - player.position;
        }
    }

    void FixedUpdate()
    {
        if (player != null)
        {
            // Determine the target position for the camera (based on the player’s current position + initial offset)
            Vector3 targetPosition = player.position + offset;

            // Use the current camera position for stabilization
            float newX = transform.position.x;
            float newY = transform.position.y;

            // Smoothly stabilize the camera on the X axis
            if (stabilizeX)
            {
                newX = Mathf.SmoothDamp(transform.position.x, targetPosition.x, ref velocityX, stabilizationSpeedX);
            }

            // Smoothly stabilize the camera on the Y axis
            if (stabilizeY)
            {
                newY = Mathf.SmoothDamp(transform.position.y, targetPosition.y, ref velocityY, stabilizationSpeedY);
            }

            // Clamp the maximum speed of the camera to prevent it from moving too fast
            Vector3 smoothedPosition = new Vector3(newX, newY, transform.position.z);
            Vector3 direction = (smoothedPosition - transform.position).normalized;
            float distance = Vector3.Distance(transform.position, smoothedPosition);

            // Apply clamping based on the maxCameraSpeed
            float cameraMoveSpeed = Mathf.Min(distance / Time.fixedDeltaTime, maxCameraSpeed);
            Vector3 finalPosition = transform.position + direction * cameraMoveSpeed * Time.fixedDeltaTime;

            // Update the camera position
            transform.position = finalPosition;
        }
    }
}
