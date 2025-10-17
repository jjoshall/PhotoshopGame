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
    [SerializeField] private float trailDelay = 0.4f;

    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private float decayAmt = 5f;
    [SerializeField] private float decayRate = 1f;

    private float currentHealth;
    private float decayTimer;
    private bool isDead = false;

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

    public void DecaySlowly() {
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

    private void UpdateHealthBar() {
        float ratio = currentHealth / maxHealth;

        Sequence sequence = DOTween.Sequence();
        sequence.Append(healthBarFillImage.DOFillAmount(ratio, 0.25f))
            .SetEase(Ease.InOutSine);
        sequence.AppendInterval(trailDelay);
        sequence.Append(healthBarTrailingFillImage.DOFillAmount(ratio, 0.3f))
            .SetEase(Ease.InOutSine);

        sequence.Play();
    }

    private void HandleDeath() {
        if (isDead) return;
        isDead = true;

        Debug.Log("You died. respawn now");

        // Trigger the event so any listener can react
        OnPlayerDeath?.Invoke();
    }
}
