using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    public static CheckpointManager Instance;

    [SerializeField] private Vector3 defaultRespawnPosition = new Vector3(18.88f, -0.49f, 0.0088f); // Default respawn position
    [SerializeField] private Vector3 currentCheckpoint; // Tracks the current active checkpoint
    private bool checkpointReached = false; // Indicates if a checkpoint has been activated

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Keep this object between scenes
        }
        else
        {
            Destroy(gameObject); // Avoid duplicate instances
        }

        // Initialize the current checkpoint to the default position
        currentCheckpoint = defaultRespawnPosition;
    }

    public void SetCheckpoint(Vector3 checkpointPosition)
    {
        // Set the active checkpoint
        currentCheckpoint = checkpointPosition;
        checkpointReached = true;
    }

    public Vector3 GetRespawnPosition()
    {
        // Return the current checkpoint if one is reached; otherwise, return the default position
        return checkpointReached ? currentCheckpoint : defaultRespawnPosition;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Set this checkpoint as the active one
            SetCheckpoint(transform.position); // Use this object's position as the checkpoint
            Debug.Log("Checkpoint Activated: " + transform.position);
        }
    }
}
