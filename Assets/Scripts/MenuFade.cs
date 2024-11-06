using System.Collections;
using UnityEngine;

public class MenuFade : MonoBehaviour
{
    public CanvasGroup mainMenu;
    public CanvasGroup optionsMenu;
    public float fadeDuration = 1f;

    public void ShowOptionsMenu()
    {
        StartCoroutine(FadeMenus(mainMenu, optionsMenu));
    }

    public void ShowMainMenu()
    {
        StartCoroutine(FadeMenus(optionsMenu, mainMenu));
    }

    private IEnumerator FadeMenus(CanvasGroup fromMenu, CanvasGroup toMenu)
    {
        float time = 0f;

        // Enable the new menu
        toMenu.alpha = 0f;
        toMenu.gameObject.SetActive(true);

        // Fade out the old menu and fade in the new menu
        while (time < fadeDuration)
        {
            time += Time.deltaTime;
            float alpha = time / fadeDuration;

            fromMenu.alpha = 1f - alpha; // Fade out
            toMenu.alpha = alpha;        // Fade in

            yield return null;
        }

        // Ensure final values are set
        fromMenu.alpha = 0f;
        fromMenu.gameObject.SetActive(false);
        toMenu.alpha = 1f;
    }

}
