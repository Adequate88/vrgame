using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FadeController : MonoBehaviour
{
    public Image fadeImage;       // Reference to the UI Image that will fade in/out.
    public float fadeDuration = 1f; // Duration of fade transition in seconds.

    private bool isFaded = false;  // False means normally visible (no fade), true means fully faded (black).

    
    // TESTING
    // void Update()
    // {
    //     // For testing purposes, we use the "E" key as our toggle button.
    //     // Replace "r" with your VR input later.
    //     if (Input.GetKeyDown(KeyCode.R))
    //     {
    //         ToggleFade();
    //         Debug.Log("Fade initialized");
    //     }
    // }

    public void ToggleFade()
    {
        if (isFaded)
        {
            // Fade out (from black to clear)
            StartCoroutine(Fade(1, 0));
            Debug.Log($"if: {isFaded}");
        }
        else
        {
            // Fade in (from clear to black)
            StartCoroutine(Fade(0, 1));
            Debug.Log($"else: {isFaded}");
        }
        isFaded = !isFaded;
        Debug.Log($"last: {isFaded}");
    }

    private IEnumerator Fade(float startAlpha, float endAlpha)
    {
        float elapsedTime = 0f;

        // Set the initial alpha
        Color imageColor = fadeImage.color;
        imageColor.a = startAlpha;
        fadeImage.color = imageColor;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / fadeDuration);
            imageColor.a = alpha;
            fadeImage.color = imageColor;
            yield return null;
        }

        // Ensure the final alpha is set correctly at the end.
        imageColor.a = endAlpha;
        fadeImage.color = imageColor;
    }
}
