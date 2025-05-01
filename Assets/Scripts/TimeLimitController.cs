using UnityEngine;
using TMPro;

public class TimeLimitController : MonoBehaviour
{
    [Header("タイムリミット（秒）")]
    public float timeLimit = 60.0f;

    [Header("残り時間表示（TextMeshPro）")]
    public TextMeshProUGUI timeText;

    public bool inputEnabled = true;

    void Start()
    {
        inputEnabled = true;
    }

    void Update()
    {
        // タイム減少（止まっていなければ）
        if (timeLimit > 0)
        {
            timeLimit -= Time.deltaTime;
            if (timeLimit <= 0)
            {
                timeLimit = 0;
                inputEnabled = false;
                Debug.Log("タイムアップ。入力無効。");
            }

            // 小数第2位まで表示
            timeText.text = "残り" + timeLimit.ToString("F1") + " 秒";
        }

        // 入力判定（有効なときだけ）
        if (inputEnabled)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Debug.Log("スペースキー入力あり");
            }
        }
    }
}
