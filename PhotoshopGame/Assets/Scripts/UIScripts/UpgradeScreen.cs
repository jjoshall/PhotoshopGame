using TMPro;
using UnityEngine;

public class UpgradeScreen : MonoBehaviour
{
    public static UpgradeScreen Instance { get; private set; }

    [SerializeField] private GameObject player;

    [SerializeField] private TextMeshProUGUI dnaAmountTxt;

    private void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void UpdateDNAText(int amount) {
        dnaAmountTxt.text = amount.ToString();
    }

    public void Upgrade1Btn() {
        Debug.Log("Upgrade 1 Button Pressed");
        DNACounter.Instance.SubtractDNA(5);
    }

    public void Upgrade2Btn() {
        Debug.Log("Upgrade 2 Button Pressed");
        DNACounter.Instance.SubtractDNA(3);
    }

    public void Upgrade3Btn() {
        Debug.Log("Upgrade 3 Button Pressed");
        DNACounter.Instance.SubtractDNA(7);
    }

    public void StartWaveBtn() {
        Debug.Log("Start Wave Button Pressed");
        player.SetActive(true);

        RockSpawner.Instance.CallWaveCoroutine();

        gameObject.SetActive(false);
    }
}
