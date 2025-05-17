using UnityEngine;
using System.Collections;

public class FadeScreen : MonoBehaviour
{
    [SerializeField] private bool fadeOnStart = true;
    [SerializeField] public float fadeDuration = 1.0f;
    [SerializeField] private Color fadeColor;

    private bool isActive = true;
    private Renderer rend;
    private MaterialPropertyBlock propertyBlock;

    void Start()
    {
        rend = GetComponent<Renderer>();
        propertyBlock = new MaterialPropertyBlock();

        if (fadeOnStart)
        {
            FadeIn();
        }
    }

    public void FadeIn()
    {
        if (!gameObject.activeSelf)
        {
            gameObject.SetActive(true);
        }
        StartCoroutine(FadeInRoutine());
    }

    private IEnumerator FadeInRoutine()
    {
        yield return FadeRoutine(1.0f, 0.0f); // Alpha from 1 (opaque) to 0 (transparent)
        gameObject.SetActive(false);
    }

    public void FadeOut()
    {
        gameObject.SetActive(true);
        StartCoroutine(FadeRoutine(0.0f, 1.0f)); // Alpha from 0 (transparent) to 1 (opaque)
    }

    public void Fade(float alphaIn, float alphaOut)
    {
        StartCoroutine(FadeRoutine(alphaIn, alphaOut));
    }

    private IEnumerator FadeRoutine(float alphaIn, float alphaOut)
    {
        float timer = 0;
        while (timer < fadeDuration)
        {
            float alpha = Mathf.Lerp(alphaIn, alphaOut, timer / fadeDuration);
            UpdateMaterialAlpha(alpha);

            timer += Time.deltaTime;
            yield return null;
        }

        UpdateMaterialAlpha(alphaOut);
    }

    private void UpdateMaterialAlpha(float alpha)
    {
        fadeColor.a = alpha;
        rend.GetPropertyBlock(propertyBlock);
        propertyBlock.SetColor("_BaseColor", fadeColor); // Use "_BaseColor" for URP shaders
        rend.SetPropertyBlock(propertyBlock);
    }
}