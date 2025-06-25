using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement; // ← シーン遷移に必要

public class HighHeightDisplay : MonoBehaviour
{
    public Transform player;
    public TextMeshProUGUI heightText;
    public TextMeshProUGUI score;
    public bool finish;

    private TimeLimitController timeLimitController;

    void Start()
    {
        // タイムリミットが終わったかの判定
        finish = false;
        timeLimitController = FindObjectOfType<TimeLimitController>();
    }

    void Update()
    {
        // タイムリミット内なら入力可
        if (timeLimitController != null && timeLimitController.inputEnabled)
        {
            float height = player.position.y / 5;
            heightText.text = "スコア：" + height.ToString("F1") + " km";
        }
        else if (!finish)
        {
            float height = player.position.y / 5;
            heightText.text = "スコア：" + height.ToString("F1") + " km";
            score.text = "スコア：" + height.ToString("F1") + " km";

            // スコアを一時保存（名前は未入力）
            PlayerPrefs.SetFloat("lastScore", height);
            PlayerPrefs.Save();

            // （任意）ランキングシーンに遷移する場合：
            SceneManager.LoadScene("RankingScene");

            finish = true;
        }
    }
}