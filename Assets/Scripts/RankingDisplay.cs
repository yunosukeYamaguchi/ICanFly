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
            Debug.Log("�X�R�A��������܂���");
            DisplayRanking();
        }
    }

    void RegisterScore()
    {
        string playerName = string.IsNullOrWhiteSpace(nameInput.text) ? "������" : nameInput.text;

        string json = PlayerPrefs.GetString("ranking", "");
        ScoreEntryList scoreList = string.IsNullOrEmpty(json) ? new ScoreEntryList() : JsonUtility.FromJson<ScoreEntryList>(json);

        var newEntry = new ScoreEntry { playerName = playerName, score = lastScore };
        scoreList.entries.Add(newEntry);
        scoreList.entries = scoreList.entries.OrderByDescending(e => e.score).Take(10).ToList();

        // �����L���O���Ŏ����̏��ʂ�T���i0�x�[�X�j
        int rank = scoreList.entries.FindIndex(e => e.playerName == playerName && e.score == lastScore) + 1;

        string updatedJson = JsonUtility.ToJson(scoreList);
        PlayerPrefs.SetString("ranking", updatedJson);
        PlayerPrefs.Save();

        // ���ʕt���ŕ\��
        var text2 = scoretext.GetComponent<TextMeshProUGUI>();
        text2.text = $"{rank}��: {playerName} - {lastScore:F1} km";

        // UI�؂�ւ�
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

        // ���������L���O�폜
        foreach (Transform child in rankingContainer)
        {
            Destroy(child.gameObject);
        }

        // �����L���O�����i���ʕt���j
        for (int i = 0; i < scoreList.entries.Count; i++)
        {
            ScoreEntry entry = scoreList.entries[i];

            GameObject entryGO = Instantiate(rankingEntryPrefab, rankingContainer);
            var text = entryGO.GetComponent<TextMeshProUGUI>();
            text.text = $"{i + 1}��: {entry.playerName} - {entry.score:F1} km";
        }
    }
}
