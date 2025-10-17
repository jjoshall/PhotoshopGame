using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;

public class PlayerDeathBar : MonoBehaviour
{
    public static PlayerDeathBar Instance { get; private set; }

    public event Action OnPlayerDeath;

    [SerializeField] private Image healthBarFillImage;
    [SerializeField] private Image healthBarTrailingFillImage;
    [SerializeField] private Image healthBarTrailBackgroundImage;
    [SerializeField] private float trailDelay = 0.4f;

    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private float decayAmt = 5f;
    [SerializeField] private float decayRate = 1f;

    [SerializeField] private float currentHealth;
    private float decayTimer;
    private bool isDead = false;

    private Tweener fillTween;
    private Tweener trailTween;

    private void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        currentHealth = maxHealth;

        healthBarFillImage.fillAmount = 1f;
        healthBarTrailingFillImage.fillAmount = 1f;
    }

    private void Update() {
        if (isDead) return;

        decayTimer += Time.unscaledDeltaTime;

        if (decayTimer >= decayRate) {
            DecaySlowly();
            decayTimer = 0f;
        }
    }

    private void DecaySlowly() {
        if (currentHealth <= 0f) {
            HandleDeath();
            return;
        }

        currentHealth -= decayAmt;
        currentHealth = Mathf.Max(currentHealth, 0f); // clamp at 0
        UpdateHealthBar();
    }

    public void TakeDamage(float damageAmt) {
        if (isDead) return;

        currentHealth -= damageAmt;
        currentHealth = Mathf.Max(currentHealth, 0f);
        if (currentHealth == 0f) {
            HandleDeath();
            return;
        }

        UpdateHealthBar();
    }

    public void AddHealth(float healthToAdd) {
        if (isDead) return;

        currentHealth += healthToAdd;
        currentHealth = Mathf.Min(currentHealth, maxHealth);

        UpdateHealthBar();
    }

    private void UpdateHealthBar() {
        float ratio = currentHealth / maxHealth;
        float currentFill = healthBarFillImage.fillAmount;

        fillTween?.Kill();
        trailTween?.Kill();

        Color healColor = Color.green;
        Color defaultColor = Color.white;

        if (ratio < currentFill) {
            // Taking damage
            healthBarTrailBackgroundImage.color = defaultColor;
            Debug.Log("Changing color to white");

            fillTween = healthBarFillImage
                .DOFillAmount(ratio, 0.25f)
                .SetEase(Ease.InOutSine);

            trailTween = healthBarTrailingFillImage
                .DOFillAmount(ratio, 0.3f)
                .SetEase(Ease.InOutSine)
                .SetDelay(trailDelay);
        }
        else {
            // Healing
            healthBarTrailBackgroundImage.color = healColor;
            Debug.Log("Changing color to green");

            trailTween = healthBarTrailingFillImage
                .DOFillAmount(ratio, 0.25f)
                .SetEase(Ease.InOutSine);

            fillTween = healthBarFillImage
                .DOFillAmount(ratio, 0.3f)
                .SetEase(Ease.InOutSine)
                .SetDelay(trailDelay);
        }
    }

    private void HandleDeath() {
        if (isDead) return;
        isDead = true;

        Debug.Log("You died. respawn now");

        // Trigger the event so any listener can react
        OnPlayerDeath?.Invoke();
    }
}
