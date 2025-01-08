using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class TriggerEventWithMusicAndFreeze : MonoBehaviour
{
    public AudioSource backgroundMusic;  // The background music AudioSource
    public AudioSource triumphantMusic;  // The triumphant music AudioSource
    public GameObject player;            // Reference to the player GameObject
    public float delayBeforeNextLevel = 2.5f;  // Time in seconds before moving to the next level
    public CanvasGroup blackoutCanvas;   // Reference to the CanvasGroup on the blackout canvas
    public float fadeDuration = 1f;      // Duration of the fade effect
    public float musicFadeDuration = 2f; // Duration of the music fade effect
    public float delayBeforeFade = 1.5f; // Delay before starting the canvas fade

    private bool isTriggered = false;
    private float initialBackgroundVolume;
    private float initialTriumphantVolume;

    private void Start()
    {
        // Store initial volumes
        initialBackgroundVolume = backgroundMusic.volume;
        initialTriumphantVolume = triumphantMusic.volume;
    }

    private void NextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isTriggered)
        {
            isTriggered = true;  // Ensure this event only happens once
            StartCoroutine(FadeOutBackgroundMusicAndFadeInTriumphantMusic());
            StartCoroutine(DelayedNextLevel());
        }
    }

    private IEnumerator DelayedNextLevel()
    {
        yield return new WaitForSeconds(delayBeforeFade); // Wait before starting the fade
        yield return FadeToBlack();
        yield return new WaitForSeconds(delayBeforeNextLevel - fadeDuration); // Adjust timing to ensure correct total delay
        NextLevel();
    }

    private IEnumerator FadeToBlack()
    {
        float startAlpha = blackoutCanvas.alpha;
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            blackoutCanvas.alpha = Mathf.Lerp(startAlpha, 1f, elapsedTime / fadeDuration);
            yield return null;
        }

        blackoutCanvas.alpha = 1f;
    }

    private IEnumerator FadeOutBackgroundMusicAndFadeInTriumphantMusic()
    {
        float elapsedTime = 0f;

        // Fade out background music
        while (elapsedTime < musicFadeDuration)
        {
            elapsedTime += Time.deltaTime;
            if (backgroundMusic != null)
            {
                backgroundMusic.volume = Mathf.Lerp(initialBackgroundVolume, 0f, elapsedTime / musicFadeDuration);
            }
            yield return null;
        }

        if (backgroundMusic != null)
        {
            backgroundMusic.Stop();
            backgroundMusic.volume = initialBackgroundVolume; // Reset volume for future use
        }

        elapsedTime = 0f;
        triumphantMusic.Play();

        // Fade in triumphant music
        while (elapsedTime < musicFadeDuration)
        {
            elapsedTime += Time.deltaTime;
            if (triumphantMusic != null)
            {
                triumphantMusic.volume = Mathf.Lerp(0f, initialTriumphantVolume, elapsedTime / musicFadeDuration);
            }
            yield return null;
        }

        if (triumphantMusic != null)
        {
            triumphantMusic.volume = initialTriumphantVolume; // Ensure final volume is set correctly
        }
    }
}
