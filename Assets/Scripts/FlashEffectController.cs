using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FlashEffectController : MonoBehaviour
{
    [Header("UI References")]
    public Image flashImage;               // The red image component to flash
    public GameObject messageTextObject;  // The "Level Failed" or "Level Passed" text GameObject

    [Header("Settings")]
    public float flashDuration = 2f;          // Total flash effect duration
    public int flashCount = 5;                // Number of flashes (fade in/out cycles)
    public float messageDisplayDuration = 3f; // Seconds to show the message before hiding everything

    private void OnEnable()
    {
        // Reset image alpha to 0.4
        SetImageAlpha(0.4f);
        messageTextObject.SetActive(false);

        StartCoroutine(FlashSequence());
    }

    private IEnumerator FlashSequence()
    {
        float halfFlashDuration = flashDuration / (flashCount * 2);

        for (int i = 0; i < flashCount; i++)
        {
            yield return StartCoroutine(FadeImageAlpha(0.4f, 0.8f, halfFlashDuration));
            yield return StartCoroutine(FadeImageAlpha(0.8f, 0.4f, halfFlashDuration));
        }

        messageTextObject.SetActive(true);

        yield return new WaitForSeconds(messageDisplayDuration);

        gameObject.SetActive(false);
    }

    private IEnumerator FadeImageAlpha(float start, float end, float duration)
    {
        float elapsed = 0f;
        Color color = flashImage.color;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(start, end, elapsed / duration);
            color.a = alpha;
            flashImage.color = color;
            yield return null;
        }

        color.a = end;
        flashImage.color = color;
    }

    private void SetImageAlpha(float alpha)
    {
        Color color = flashImage.color;
        color.a = alpha;
        flashImage.color = color;
    }
}
