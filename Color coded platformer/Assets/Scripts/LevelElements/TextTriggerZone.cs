using UnityEngine;
using TMPro;
using System.Collections;

public class TextTriggerZone : MonoBehaviour
{
    public TextMeshPro targetText;
    public float fadeSpeed = 2f;

    private void Awake()
    {
        if (targetText != null) 
        {
            Color c = targetText.color;
            c.a = 0;
            targetText.color = c;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) { StopAllCoroutines(); StartCoroutine(FadeText(1f)); }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) { StopAllCoroutines(); StartCoroutine(FadeText(0f)); }
    }

    IEnumerator FadeText(float targetAlpha)
    {
        if (targetText == null) yield break;
        while (Mathf.Abs(targetText.color.a - targetAlpha) > 0.05f)
        {
            Color c = targetText.color;
            c.a = Mathf.Lerp(c.a, targetAlpha, Time.deltaTime * fadeSpeed);
            targetText.color = c;
            yield return null;
        }
    }
}
