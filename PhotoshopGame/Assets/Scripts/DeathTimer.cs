using UnityEngine;
using UnityEngine.UI;

public class DeathTimer : MonoBehaviour
{
    public static DeathTimer Instance { get; private set; }

    [SerializeField] private Slider timeSlider;
    [SerializeField] private float addTimeFromKill = 1.5f;
    private float currentTime;

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
            // For demonstration, we'll just reset the timer
            currentTime = timeSlider.maxValue;
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
