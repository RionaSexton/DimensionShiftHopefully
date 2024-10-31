using UnityEngine;
using System.Collections.Generic;

public class KeyAndDoorScript : MonoBehaviour
{
    private Dictionary<string, bool> keysCollected = new Dictionary<string, bool>()
    {
        { "Red", false },
        { "Green", false }
    };

    private float teleportCooldown = 1.0f; // 1-second cooldown to prevent rapid teleports
    private float lastTeleportTime = -1.0f; // Time of the last teleport

    private Camera mainCamera; // Reference to the main camera

    private void Start()
    {
        mainCamera = Camera.main; // Initialize main camera reference
    }

    private void OnTriggerEnter2D(Collider2D other) // 2D trigger for 2D game
    {
        if (Time.time - lastTeleportTime < teleportCooldown) return;

        // Key pickup logic
        if (other.CompareTag("RedKey"))
        {
            keysCollected["Red"] = true;
            Destroy(other.gameObject);
        }
        else if (other.CompareTag("GreenKey"))
        {
            keysCollected["Green"] = true;
            Destroy(other.gameObject);
        }

        // Two-way teleportation between RedDoor1 and RedDoor2
        else if (other.CompareTag("RedDoor1") && keysCollected["Red"])
        {
            Transform redDoor2 = GameObject.FindGameObjectWithTag("RedDoor2")?.transform;
            if (redDoor2 != null)
            {
                TeleportPlayer(redDoor2);
            }
            else
            {
                Debug.LogWarning("RedDoor2 not found or incorrectly tagged!");
            }
        }
        else if (other.CompareTag("RedDoor2") && keysCollected["Red"])
        {
            Transform redDoor1 = GameObject.FindGameObjectWithTag("RedDoor1")?.transform;
            if (redDoor1 != null)
            {
                TeleportPlayer(redDoor1);
            }
            else
            {
                Debug.LogWarning("RedDoor1 not found or incorrectly tagged!");
            }
        }

        // One-way teleportation from RedDoor3 to RedDoor2
        else if (other.CompareTag("RedDoor3") && keysCollected["Red"])
        {
            Transform redDoor2 = GameObject.FindGameObjectWithTag("RedDoor2")?.transform;
            if (redDoor2 != null)
            {
                TeleportPlayer(redDoor2);
            }
            else
            {
                Debug.LogWarning("RedDoor2 not found or incorrectly tagged!");
            }
        }
    }

    private void TeleportPlayer(Transform targetDoor)
    {
        // Teleport the player to the target door's position
        transform.position = targetDoor.position;
        lastTeleportTime = Time.time;

        // Move the camera to match the player's new position
        if (mainCamera != null)
        {
            Vector3 newCameraPosition = targetDoor.position;
            newCameraPosition.z = mainCamera.transform.position.z; // Keep original z-position of the camera
            mainCamera.transform.position = newCameraPosition;
        }
    }
}
