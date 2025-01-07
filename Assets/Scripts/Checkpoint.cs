using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public string playerTag = "Player";
    public GameObject visualCue;

    private void Start()
    {
        // Ensure the visual cue is hidden at the start
        if (visualCue != null)
        {
            visualCue.SetActive(false);
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(playerTag))
        {
            // Update the current checkpoint in CheckpointManager
            CheckpointManager.Instance.SetCheckpoint(transform.position);
            Debug.Log("Checkpoint activated at: " + transform.position);

            if (visualCue != null)
            {
                visualCue.SetActive(true);
            }
        }
    }
}
