using UnityEngine;

public class Bobbing : MonoBehaviour
{
    public float bobbingSpeed = 2f;  // Speed of the bobbing motion
    public float bobbingHeight = 0.5f;  // Height of the bobbing motion

    private Vector3 startPosition;  // The starting position of the object

    void Start()
    {
        // Store the object's starting position
        startPosition = transform.position;
    }

    void Update()
    {
        // Calculate the new Y position using a sine wave for smooth bobbing
        float newY = startPosition.y + Mathf.Sin(Time.time * bobbingSpeed) * bobbingHeight;

        // Update the object's position
        transform.position = new Vector3(startPosition.x, newY, startPosition.z);
    }
}
