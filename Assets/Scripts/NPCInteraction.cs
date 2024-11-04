using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class NPCInteraction : MonoBehaviour
{
    public TextMeshProUGUI promptText; // Reference to the "Press E to talk" text UI
    public TextMeshProUGUI dialogueText; // Reference to the dialogue text UI
    public RawImage speechBubble; // RawImage for speech bubble
    public Animator npcAnimator; // Animator for the NPC

    private bool isPlayerNearby = false;

    void Start()
    {
        // Ensure texts are hidden initially
        promptText.gameObject.SetActive(false);
        dialogueText.gameObject.SetActive(false);
        speechBubble.gameObject.SetActive(false);
        // Ensure the NPC is in the idle state at start
        npcAnimator.SetBool("isTalking", false);
    }

    void Update()
    {
        // Show dialogue when pressing "E" near the NPC
        if (isPlayerNearby && Input.GetKeyDown(KeyCode.E))
        {
            ShowDialogue();
        }
    }

    void ShowDialogue()
    {
        dialogueText.gameObject.SetActive(true);
        promptText.gameObject.SetActive(false);
        speechBubble.gameObject.SetActive(true);

        // Set the Animator to play the talking animation
        npcAnimator.SetBool("isTalking", true);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = true;
            promptText.gameObject.SetActive(true); // Show "Press E to talk" prompt
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = false;
            promptText.gameObject.SetActive(false); // Hide the prompt
            dialogueText.gameObject.SetActive(false); // Hide dialogue when leaving
            speechBubble.gameObject.SetActive(false); // Hide speech bubble when leaving

            // Set the Animator to play the idle animation
            npcAnimator.SetBool("isTalking", false);
        }
    }
}
