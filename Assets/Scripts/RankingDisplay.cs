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

    private float lastScore;

    void Start()
    {
        lastScore = PlayerPrefs.GetFloat("lastScore", -1f);
        submitButton.onClick.AddListener(RegisterScore);

        if (lastScore < 0)
        {
            Debug.Log("ƒXƒRƒA‚ªŒ©‚Â‚©‚è‚Ü‚¹‚ñ");
            DisplayRanking();
        }
    }

    void RegisterScore()
    {
        string playerName = string.IsNullOrWhiteSpace(nameInput.text) ? "–¼–³‚µ" : nameInput.text;

        string json = PlayerPrefs.GetString("ranking", "");
        ScoreEntryList scoreList = string.IsNullOrEmpty(json) ? new ScoreEntryList() : JsonUtility.FromJson<ScoreEntryList>(json);

        scoreList.entries.Add(new ScoreEntry { playerName = playerName, score = lastScore });
        scoreList.entries = scoreList.entries.OrderByDescending(e => e.score).Take(10).ToList();

        string updatedJson = JsonUtility.ToJson(scoreList);
        PlayerPrefs.SetString("ranking", updatedJson);
        PlayerPrefs.Save();

        nameInput.gameObject.SetActive(false);
        submitButton.gameObject.SetActive(false);

        DisplayRanking();
        inputimg.SetActive(false);
        rankimg.SetActive(true);
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

        foreach (ScoreEntry entry in scoreList.entries)
        {
            GameObject entryGO = Instantiate(rankingEntryPrefab, rankingContainer);
            var text = entryGO.GetComponent<TextMeshProUGUI>();
            text.text = $"{entry.playerName} - {entry.score:F1} km";
            var text2 = scoretext.GetComponent<TextMeshProUGUI>();
            text2.text = $"{entry.playerName} - {entry.score:F1} km";
        }
    }
}