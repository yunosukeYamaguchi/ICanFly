using UnityEngine;

public class GameManager : MonoBehaviour
{
    public CanvasGroup canvasGroup;
    public GameObject ana;
    public GameObject[] se;

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

        ana.SetActive(false);
        for (int i = 0; i < se.Length; i++)
        {
            if (se[i] != null) se[i].SetActive(false);
        }
        se[3].SetActive(true);
    }

    public void ResultScene()
    {
        StartCoroutine(FadeIn());
    }

    private System.Collections.IEnumerator FadeIn()
    {
        float elapsed = 0f;
        ana.SetActive(true);
        se[1].SetActive(true);
        se[2].SetActive(true);
        se[3].SetActive(false);

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            canvasGroup.alpha = Mathf.Clamp01(elapsed / fadeDuration);
            yield return null;
        }

        canvasGroup.alpha = 1f; // 念のため完全表示
    }
}
