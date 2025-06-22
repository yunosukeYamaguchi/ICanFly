using UnityEngine;
using TMPro;

public class HighHeightDisplay : MonoBehaviour
{
    public Transform player;
    public TextMeshProUGUI heightText;
    public TextMeshProUGUI score;
    public bool finish;

    private TimeLimitController timeLimitController;

    void Start()
    {
        //タイムリミットが終わったかの判定
        finish = false;

        timeLimitController = FindObjectOfType<TimeLimitController>();
    }

    void Update()
    {
        //タイムリミット内なら入力可
        if (timeLimitController != null && timeLimitController.inputEnabled)
        {
            float height = player.position.y / 5;
            heightText.text = "最高到達点：" + height.ToString("F1") + " km";
        }
        else if (!finish)
        {
            float height = player.position.y / 5;
            heightText.text = "最高到達点：" + height.ToString("F1") + " km";
            score.text = "スコア：" + height.ToString("F1") + " km";

            finish = true;
        }
    }
}