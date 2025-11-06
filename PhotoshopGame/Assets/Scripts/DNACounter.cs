using UnityEngine;
using TMPro;

public class DNACounter : MonoBehaviour
{
    public static DNACounter Instance { get; private set; }

    [SerializeField] private TextMeshProUGUI inGameDnaTxt;
    [SerializeField] private TextMeshProUGUI upgradeScreenDnaTxt;
    private int dnaCount = 0;

    private void Awake() {
        Instance = this;
        UpdateText();
    }

    public void AddDNA(int amount) {
        dnaCount += amount;
        UpdateText();
    }

    public void SubtractDNA(int amount) {
        dnaCount -= amount;
        UpdateText();
    }

    private void UpdateText() {
        inGameDnaTxt.text = dnaCount.ToString();
        upgradeScreenDnaTxt.text = dnaCount.ToString();
    }
}
