using UnityEngine;
using UnityEngine.UI; // For handling UI components
using System.Collections; // For using Coroutines

public class DimensionShift : MonoBehaviour
{
    public string dimensionALayerName = "DimensionA"; // Name of the Dimension A layer
    public string dimensionBLayerName = "DimensionB"; // Name of the Dimension B layer
    public string dimensionATag = "DimensionATag";    // Tag for Dimension A objects
    public string dimensionBTag = "DimensionBTag";    // Tag for Dimension B objects

    private GameObject[] dimensionAObjects; // All objects in Dimension A
    private GameObject[] dimensionBObjects; // All objects in Dimension B
    private bool inDimensionA = true; // Keep track of the current dimension

    public Image blackoutImage; // Reference to the UI Image used for blackout
    public float blackoutDuration = 0.5f; // Duration for fade in/out

    void Start()
    {
        // Get all objects in the DimensionA and DimensionB layers or with tags
        dimensionAObjects = FindObjectsInDimension(LayerMask.NameToLayer(dimensionALayerName), dimensionATag);
        dimensionBObjects = FindObjectsInDimension(LayerMask.NameToLayer(dimensionBLayerName), dimensionBTag);

        // Initialize the game state: Start in Dimension A
        SetDimensionAActive(true);
        SetDimensionBActive(false);

        // Set the blackout image to be transparent initially
        blackoutImage.color = new Color(0, 0, 0, 0);
    }

    void Update()
    {
        // Listen for the E key press to switch dimensions
        if (Input.GetMouseButtonDown(1))
        {
            StartCoroutine(SwitchDimensionWithBlackout());
        }
    }

    // Coroutine to handle the blackout and dimension switch
    IEnumerator SwitchDimensionWithBlackout()
    {
        // Fade to black
        yield return StartCoroutine(FadeToBlack());

        // Switch dimensions after blackout
        ToggleDimension();

        // Fade back to transparent
        yield return StartCoroutine(FadeToClear());
    }

    // Method to toggle between Dimension A and Dimension B
    void ToggleDimension()
    {
        if (inDimensionA)
        {
            // Switch to Dimension B
            SetDimensionAActive(false);
            SetDimensionBActive(true);
        }
        else
        {
            // Switch to Dimension A
            SetDimensionAActive(true);
            SetDimensionBActive(false);
        }

        inDimensionA = !inDimensionA; // Toggle the dimension flag
    }

    // Function to find all objects in a given layer or tag
    GameObject[] FindObjectsInDimension(int layer, string tag)
    {
        // Find all objects in the scene
        GameObject[] allObjects = GameObject.FindObjectsOfType<GameObject>();

        // Filter objects either by layer or by tag
        return System.Array.FindAll(allObjects, obj => obj.layer == layer || obj.CompareTag(tag));
    }

    // Method to activate or deactivate all objects in Dimension A
    void SetDimensionAActive(bool isActive)
    {
        foreach (GameObject obj in dimensionAObjects)
        {
            obj.SetActive(isActive);
        }
    }

    // Method to activate or deactivate all objects in Dimension B
    void SetDimensionBActive(bool isActive)
    {
        foreach (GameObject obj in dimensionBObjects)
        {
            obj.SetActive(isActive);
        }
    }

    // Coroutine to fade the screen to black
    IEnumerator FadeToBlack()
    {
        float elapsedTime = 0f;
        while (elapsedTime < blackoutDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Clamp01(elapsedTime / blackoutDuration);
            blackoutImage.color = new Color(0, 0, 0, alpha);
            yield return null;
        }
    }

    // Coroutine to fade the screen back to clear (transparent)
    IEnumerator FadeToClear()
    {
        float elapsedTime = 0f;
        while (elapsedTime < blackoutDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = 1f - Mathf.Clamp01(elapsedTime / blackoutDuration);
            blackoutImage.color = new Color(0, 0, 0, alpha);
            yield return null;
        }
    }
}
