using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    public static CheckpointManager Instance { get; private set; }

    private Vector3 respawnPosition;
    private bool hasCheckpoint;

    private void Awake()
    {
        // Singleton pattern for global access
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        // Initialize with no checkpoint
        respawnPosition = Vector3.zero;
        hasCheckpoint = false;
    }

    public void SetCheckpoint(Vector3 position)
    {
        respawnPosition = position;
        hasCheckpoint = true;
    }

    public Vector3 GetRespawnPosition()
    {
        return hasCheckpoint ? respawnPosition : Vector3.zero;
    }

    public bool HasCheckpoint()
    {
        return hasCheckpoint;
    }
}
