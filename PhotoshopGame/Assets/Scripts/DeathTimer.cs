using UnityEngine;
using UnityEngine.UI;

public class DeathTimer : MonoBehaviour
{
    public static DeathTimer Instance { get; private set; }

    [SerializeField] private Slider timeSlider;
    [SerializeField] private float addTimeFromKill = 1.5f;
    private float currentTime;

    [SerializeField] private GameObject deathScreen;

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
        currentTime -= Time.unscaledDeltaTime;
        timeSlider.value = currentTime;
        if (currentTime <= 0f) {
            // Handle player death (e.g., reload scene, show game over screen, etc.)
            Debug.Log("Player Died!");
            Time.timeScale = 0f; // Pause the game

            deathScreen.SetActive(true); // Show the death screen
        }
    }

    public void AddTime() {
        currentTime += addTimeFromKill;
        if (currentTime > timeSlider.maxValue) {
            currentTime = timeSlider.maxValue;
        }
        timeSlider.value = currentTime;
    }
}
