using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public string playerTag = "Player";

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(playerTag))
        {
            // Update the current checkpoint in CheckpointManager
            CheckpointManager.Instance.SetCheckpoint(transform.position);
            Debug.Log("Checkpoint activated at: " + transform.position);
        }
    }
}
