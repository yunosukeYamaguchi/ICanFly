using UnityEngine;
using TMPro;

public class TimeLimitController : MonoBehaviour
{
    [Header("タイムリミット（秒）")]
    public float timeLimit = 60.0f;

    [Header("残り時間表示（TextMeshPro）")]
    public TextMeshProUGUI timeText;

    [Header("プレイヤーのSpriteRenderer")]
    public SpriteRenderer playerSpriteRenderer; // プレイヤーのSpriteRendererを指定

    [Header("警告用の画像スプライト")]
    public Sprite warningSprite; // 残り5秒で切り替える画像

    private bool hasSwitchedImage = false; // 一度だけ切り替える用

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

            // 画像切り替え（5秒を切った瞬間に1度だけ）
            if (!hasSwitchedImage && timeLimit <= 5f)
            {
                if (playerSpriteRenderer != null && warningSprite != null)
                {
                    playerSpriteRenderer.sprite = warningSprite;
                    hasSwitchedImage = true;
                    Debug.Log("警告スプライトに切り替え");
                }
            }

            if (timeLimit <= 0)
            {
                timeLimit = 0;
                inputEnabled = false;
                Debug.Log("タイムアップ。入力無効。");
            }

            // 小数第1位まで表示
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
