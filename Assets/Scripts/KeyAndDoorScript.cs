using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class KeyAndDoorScript : MonoBehaviour
{
    private Dictionary<string, bool> keysCollected = new Dictionary<string, bool>()
    {
        { "Red", false },
        { "Green", false }
    };

    private float teleportCooldown = 3.0f; // 3-second cooldown to prevent rapid teleports
    private float lastTeleportTime = -3.0f; // Time of the last teleport

    private Camera mainCamera; // Reference to the main camera

    // Audio components
    public AudioSource audioSource; // Reference to an AudioSource component
    public AudioClip redKeyClip; // Audio clip for red key
    public AudioClip greenKeyClip; // Audio clip for green key

    // UI Images for the keys
    public GameObject redKeyImage; // UI image for the red key
    public GameObject greenKeyImage; // UI image for the green key

    private void Start()
    {
        mainCamera = Camera.main; // Initialize main camera reference
        if (audioSource == null)
        {
            Debug.LogWarning("AudioSource is not assigned!");
        }

        // Ensure key images are initially inactive
        if (redKeyImage != null) redKeyImage.SetActive(false);
        if (greenKeyImage != null) greenKeyImage.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other) // 2D trigger for 2D game
    {
        if (Time.time - lastTeleportTime < teleportCooldown) return;

        // Key pickup logic
        if (other.CompareTag("RedKey"))
        {
            keysCollected["Red"] = true;
            PlayKeySound(redKeyClip);
            ActivateKeyImage(redKeyImage);
            Destroy(other.gameObject);
        }
        else if (other.CompareTag("GreenKey"))
        {
            keysCollected["Green"] = true;
            PlayKeySound(greenKeyClip);
            ActivateKeyImage(greenKeyImage);
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

        // Two-way teleportation between GreenDoor1 and GreenDoor2
        else if (other.CompareTag("GreenDoor1") && keysCollected["Green"])
        {
            Transform greenDoor2 = GameObject.FindGameObjectWithTag("GreenDoor2")?.transform;
            if (greenDoor2 != null)
            {
                TeleportPlayer(greenDoor2);
            }
            else
            {
                Debug.LogWarning("GreenDoor2 not found or incorrectly tagged!");
            }
        }
        else if (other.CompareTag("GreenDoor2") && keysCollected["Green"])
        {
            Transform greenDoor1 = GameObject.FindGameObjectWithTag("GreenDoor1")?.transform;
            if (greenDoor1 != null)
            {
                TeleportPlayer(greenDoor1);
            }
            else
            {
                Debug.LogWarning("GreenDoor1 not found or incorrectly tagged!");
            }
        }
    }

    private void PlayKeySound(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
        else
        {
            Debug.LogWarning("AudioSource or AudioClip is missing!");
        }
    }

    private void ActivateKeyImage(GameObject keyImage)
    {
        if (keyImage != null)
        {
            keyImage.SetActive(true); // Activate the UI image
        }
        else
        {
            Debug.LogWarning("Key image GameObject is not assigned!");
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
