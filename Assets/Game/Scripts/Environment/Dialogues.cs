using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Dialogues : MonoBehaviour
{
    public Image popupImage;           // Drag your Image component here
    public float fadeDuration = 0.5f;  // Time to fade in/out

    private Coroutine fadeCoroutine;

    private void Awake()
    {
        if (popupImage != null)
        {
            Color c = popupImage.color;
            c.a = 0f;                  // start invisible
            popupImage.color = c;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && popupImage != null)
        {
            if (fadeCoroutine != null) StopCoroutine(fadeCoroutine);
            fadeCoroutine = StartCoroutine(FadeImage(popupImage, 1f, fadeDuration));
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && popupImage != null)
        {
            if (fadeCoroutine != null) StopCoroutine(fadeCoroutine);
            fadeCoroutine = StartCoroutine(FadeImage(popupImage, 0f, fadeDuration));
        }
    }

    private IEnumerator FadeImage(Image image, float targetAlpha, float duration)
    {
        float startAlpha = image.color.a;
        float time = 0f;

        while (time < duration)
        {
            time += Time.deltaTime;
            Color c = image.color;
            c.a = Mathf.Lerp(startAlpha, targetAlpha, time / duration);
            image.color = c;
            yield return null;
        }

        Color final = image.color;
        final.a = targetAlpha;
        image.color = final;
    }
}
