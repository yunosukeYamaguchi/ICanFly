using UnityEngine;

public class GameManager : MonoBehaviour
{
    public CanvasGroup canvasGroup;

    public float fadeDuration = 1f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (canvasGroup == null)
        {
            Debug.LogError("CanvasGroupがアタッチされていません");
            return;
        }

        canvasGroup.alpha = 0f; // 最初は透明
    }

    public void ResultScene()
    {
        StartCoroutine(FadeIn());
    }

    private System.Collections.IEnumerator FadeIn()
    {
        float elapsed = 0f;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            canvasGroup.alpha = Mathf.Clamp01(elapsed / fadeDuration);
            yield return null;
        }

        canvasGroup.alpha = 1f; // 念のため完全表示
    }
}
