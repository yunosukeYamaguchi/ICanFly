using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System.Linq;

public class RankingDisplay : MonoBehaviour
{
    [System.Serializable]
    public class ScoreEntry
    {
        public string playerName;
        public float score;
    }

    [System.Serializable]
    public class ScoreEntryList
    {
        public List<ScoreEntry> entries = new List<ScoreEntry>();
    }

    public TMP_InputField nameInput;
    public Button submitButton;
    public Transform rankingContainer;
    public GameObject rankingEntryPrefab;
    public GameObject scoretext;
    public GameObject inputimg;
    public GameObject rankimg;
    public GameObject Ranking;

    private float lastScore;

    void Start()
    {
        lastScore = PlayerPrefs.GetFloat("lastScore", -1f);
        submitButton.onClick.RemoveAllListeners();
        submitButton.onClick.AddListener(RegisterScore);

        if (lastScore < 0)
        {
            Debug.Log("スコアが記録されていません");
            DisplayRanking();
        }
    }

    void Update()
    {
        // Escapeキーでランキングデータを削除
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PlayerPrefs.DeleteKey("ranking");
            PlayerPrefs.Save();
            Debug.Log("ランキングデータを削除しました");
            
            // 表示されているランキングを消す
            foreach (Transform child in rankingContainer)
            {
                Destroy(child.gameObject);
            }
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            RegisterScore();

            Ranking.SetActive(true);
        }
        
    }

    void RegisterScore()
    {
        string playerName = string.IsNullOrWhiteSpace(nameInput.text) ? "名無し" : nameInput.text;

        string json = PlayerPrefs.GetString("ranking", "");
        ScoreEntryList scoreList = string.IsNullOrEmpty(json) ? new ScoreEntryList() : JsonUtility.FromJson<ScoreEntryList>(json);

        var newEntry = new ScoreEntry { playerName = playerName, score = lastScore };
        scoreList.entries.Add(newEntry);
        scoreList.entries = scoreList.entries.OrderByDescending(e => e.score).Take(100).ToList();

        int rank = scoreList.entries.FindIndex(e => e.playerName == playerName && e.score == lastScore) + 1;

        string updatedJson = JsonUtility.ToJson(scoreList);
        PlayerPrefs.SetString("ranking", updatedJson);
        PlayerPrefs.Save();

        var text2 = scoretext.GetComponent<TextMeshProUGUI>();
        text2.text = $"{rank} : {playerName} - {lastScore:F1} km";

        nameInput.gameObject.SetActive(false);
        submitButton.gameObject.SetActive(false);
        inputimg.SetActive(false);
        rankimg.SetActive(true);

        DisplayRanking();
    }

    void DisplayRanking()
    {
        string json = PlayerPrefs.GetString("ranking", "");
        if (string.IsNullOrEmpty(json)) return;

        ScoreEntryList scoreList = JsonUtility.FromJson<ScoreEntryList>(json);

        foreach (Transform child in rankingContainer)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < scoreList.entries.Count; i++)
        {
            ScoreEntry entry = scoreList.entries[i];

            GameObject entryGO = Instantiate(rankingEntryPrefab, rankingContainer);
            var text = entryGO.GetComponent<TextMeshProUGUI>();
            text.text = $"{i + 1} : {entry.playerName} - {entry.score:F1} km";
        }
    }
}
