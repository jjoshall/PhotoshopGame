using UnityEngine;
using System.Collections;

public class HitStop : MonoBehaviour
{
    public static HitStop Instance { get; private set; }

    [SerializeField] private float defaultTimeScale = 1f;

    private bool isHitStopping = false;

    private void Awake() {
        // Basic singleton pattern
        if (Instance != null && Instance != this) {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    /// Trigger a hitstop for a set duration (in seconds).
    public void Stop(float duration, float slowdownFactor = 0f) {
        if (!isHitStopping)
            StartCoroutine(HitStopRoutine(duration, slowdownFactor));
    }

    private IEnumerator HitStopRoutine(float duration, float slowdownFactor) {
        isHitStopping = true;
        float originalTimeScale = Time.timeScale;

        Time.timeScale = slowdownFactor;
        yield return new WaitForSecondsRealtime(duration);

        Time.timeScale = defaultTimeScale;
        isHitStopping = false;
    }
}
