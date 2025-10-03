using UnityEngine;
using UnityEngine.UI;

public class DeathTimer : MonoBehaviour
{
    public static DeathTimer Instance { get; private set; }

    [SerializeField] private Slider timeSlider;
    [SerializeField] private float addTimeFromKill = 1.5f;
    private float currentTime;

    private bool isGameOver = false;

    [SerializeField] private GameObject deathScreen;
    [SerializeField] private RockCollision rockCollision; // Reference to RockCollision script
    [SerializeField] private HighscoreTable highscoreTable; // Reference to HighscoreTable script

    private void Awake() {
        // Basic singleton pattern
        if (Instance != null && Instance != this) {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start() {
        currentTime = timeSlider.maxValue;
        timeSlider.value = currentTime;
    }

    private void Update() {
        if (isGameOver) return;

        currentTime -= Time.unscaledDeltaTime;
        timeSlider.value = currentTime;
        if (currentTime <= 0f) {
            currentTime = 0f;
            isGameOver = true;
            Time.timeScale = 0f; // Pause the game
            OnGameOver();
        }
    }

    public void OnGameOver() {
        int finalScore = rockCollision.CurrentScore;
        string playerName = "YOU"; // You can modify this to get the player's name from input

        highscoreTable.AddHighScoreEntry(finalScore, playerName);

        deathScreen.SetActive(true);
    }

    public void AddTime() {
        currentTime += addTimeFromKill;
        if (currentTime > timeSlider.maxValue) {
            currentTime = timeSlider.maxValue;
        }
        timeSlider.value = currentTime;
    }
}
