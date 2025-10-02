using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HighscoreTable : MonoBehaviour
{
    private Transform entryContainer;
    private Transform entryTemplate;
    private List<Transform> highscoreEntryTransformList;

    private void Awake() {
        entryContainer = transform.Find("HighscoreEntryContainer");
        entryTemplate = entryContainer.Find("HighscoreEntryTemplate");

        entryTemplate.gameObject.SetActive(false);

        string jsonString = PlayerPrefs.GetString("highscoreTable", ""); // "" if not found
        Highscores highscores = JsonUtility.FromJson<Highscores>(jsonString);

        if (highscores == null || highscores.highscoreEntryList == null) {
            // No saved data → use default
            highscores.highscoreEntryList = new List<HighscoreEntry>() {
        new HighscoreEntry { score = 12498, name = "AAA" },
        new HighscoreEntry { score = 123523, name = "BBB" },
        new HighscoreEntry { score = 5468, name = "CCC" },
        new HighscoreEntry { score = 657, name = "DDD" },
        new HighscoreEntry { score = 67956, name = "EEE" },
        new HighscoreEntry { score = 675623, name = "FFF" },
        new HighscoreEntry { score = 678435, name = "GGG" },
        new HighscoreEntry { score = 2534, name = "HHH" },
        new HighscoreEntry { score = 2134, name = "III" }
    };

            // Save the defaults so next time PlayerPrefs has something
            Highscores newHighscores = new Highscores { highscoreEntryList = highscores.highscoreEntryList };
            string json = JsonUtility.ToJson(newHighscores);
            PlayerPrefs.SetString("highscoreTable", json);
            PlayerPrefs.Save();
        }
        else {
            // Load from saved data
            highscores.highscoreEntryList = highscores.highscoreEntryList;
        }

        for (int i = 0; i < highscores.highscoreEntryList.Count; i++) {
            for (int j = i + 1; j < highscores.highscoreEntryList.Count; j++) {
                if (highscores.highscoreEntryList[j].score > highscores.highscoreEntryList[i].score) {
                    HighscoreEntry tmp = highscores.highscoreEntryList[i];
                    highscores.highscoreEntryList[i] = highscores.highscoreEntryList[j];
                    highscores.highscoreEntryList[j] = tmp;
                }
            }
        }

        highscoreEntryTransformList = new List<Transform>();
        foreach (HighscoreEntry highscoreEntry in highscores.highscoreEntryList) {
            CreateHighscoreEntryTransform(highscoreEntry, entryContainer, highscoreEntryTransformList);
        }
    }

    private void CreateHighscoreEntryTransform(HighscoreEntry highscoreEntry,Transform container, List<Transform> transformList) {
        float templateHeight = 30f;

        Transform entryTransform = Instantiate(entryTemplate, container);
        RectTransform entryRectTransform = entryTransform.GetComponent<RectTransform>();
        entryRectTransform.anchoredPosition = new Vector2(0, -templateHeight * transformList.Count);
        entryTransform.gameObject.SetActive(true);

        int rank = transformList.Count + 1;
        string rankString;
        switch (rank) {
            default:
                rankString = rank + "TH";
                break;
            case 1:
                rankString = "1ST";
                break;
            case 2:
                rankString = "2ND";
                break;
            case 3:
                rankString = "3RD";
                break;
        }

        entryTransform.Find("PosTxt").GetComponent<TextMeshProUGUI>().text = rankString;

        int score = highscoreEntry.score;
        entryTransform.Find("ScoreTxt").GetComponent<TextMeshProUGUI>().text = score.ToString();

        string name = highscoreEntry.name;
        entryTransform.Find("NameTxt").GetComponent<TextMeshProUGUI>().text = name;

        entryTransform.Find("Background").gameObject.SetActive(rank % 2 == 1);
        
        if (rank == 1) {
            entryTransform.Find("PosTxt").GetComponent<TextMeshProUGUI>().color = Color.yellow;
            entryTransform.Find("ScoreTxt").GetComponent<TextMeshProUGUI>().color = Color.yellow;
            entryTransform.Find("NameTxt").GetComponent<TextMeshProUGUI>().color = Color.yellow;
        }

        transformList.Add(entryTransform);
    }

    private void AddHighScoreEntry(int score, string name) {
        HighscoreEntry highscoreEntry = new HighscoreEntry { score = score, name = name };

        string jsonString = PlayerPrefs.GetString("highscoreTable", ""); // "" if not found
        Highscores highscores = JsonUtility.FromJson<Highscores>(jsonString);

        highscores.highscoreEntryList.Add(highscoreEntry);

        string json = JsonUtility.ToJson(highscores);
        PlayerPrefs.SetString("highscoreTable", json);
        PlayerPrefs.Save();
    }

    private class Highscores {
        public List<HighscoreEntry> highscoreEntryList;
    }

    [System.Serializable]
    private class HighscoreEntry {
        public int score;
        public string name;
    }
}
