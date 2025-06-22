using System.Collections;
using TMPro;
using UnityEngine;
using TMPro;

public class GameStartCountdown : MonoBehaviour
{
    public TextMeshProUGUI countdownText; // TextMeshProUGUIの場合は public TMP_Text countdownText;
    public float countdownTime = 3f; // カウントの開始時間

    public GameObject game;
    public GameObject countdown;

    void Start()
    {
        game.SetActive(false);
        StartCoroutine(CountdownRoutine());
    }

    IEnumerator CountdownRoutine()
    {
        float time = countdownTime;

        while (time > 0)
        {
            countdownText.text = Mathf.Ceil(time).ToString(); // 3, 2, 1...
            yield return new WaitForSeconds(1f);
            time -= 1f;
        }

        countdownText.text = "GO!";
        yield return new WaitForSeconds(1f);

        countdownText.gameObject.SetActive(false); // 非表示にする

        // ゲーム開始処理を呼び出す（必要に応じて）
        if (game != null)
        {
            game.SetActive(true);
            countdown.SetActive(false);
        }
    }
}
