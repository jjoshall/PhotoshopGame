using UnityEngine;
using TMPro;

public class DNACounter : MonoBehaviour
{
    public static DNACounter Instance { get; private set; }

    [SerializeField] private TextMeshProUGUI dnaTxt;
    private int dnaCount = 0;

    private void Awake() {
        Instance = this;
        UpdateText();
    }

    public void AddDNA() {
        dnaCount++;
        UpdateText();
    }

    private void UpdateText() {
        dnaTxt.text = dnaCount.ToString();
    }
}
