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
            float height = player.position.y;
            heightText.text = "最高到達点：" + height.ToString("F1") + " m";
        }
        else if (!finish)
        {
            float height = player.position.y;
            heightText.text = "最高到達点：" + height.ToString("F1") + " m";
            score.text = "最高到達点：" + height.ToString("F1") + " m";

            finish = true;
        }
    }
}