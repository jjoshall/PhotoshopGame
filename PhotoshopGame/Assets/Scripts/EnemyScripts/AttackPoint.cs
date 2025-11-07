using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class AttackPoint : MonoBehaviour
{
    public static AttackPoint Instance { get; private set; }

    [SerializeField] private SpriteRenderer spriteRenderer;

    [SerializeField] private Image healthBarFillImage;
    [SerializeField] private Image healthBarTrailingFillImage;
    [SerializeField] private float trailDelay = 0.4f;

    [SerializeField] private GameObject deathScreen;

    [SerializeField] private float maxHealth = 100f;

    private float currentHealth;

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

    public void TakeDamage(float damageAmt) {
        if (currentHealth <= 0f) {
            currentHealth = 0f;
            Time.timeScale = 0f;

            deathScreen.SetActive(true);

            CanvasGroup cg = deathScreen.GetComponent<CanvasGroup>();
            cg.alpha = 0f;

            cg.DOFade(1f, 1f).SetUpdate(true);
            return;
        }

        FlashWhite();
        Shake();
        UpdateHealthBar(damageAmt);
    }

    private void UpdateHealthBar(float damageAmt) {
        currentHealth -= damageAmt;
        float ratio = currentHealth / maxHealth;

        Sequence sequence = DOTween.Sequence();
        sequence.Append(healthBarFillImage.DOFillAmount(ratio, 0.25f))
            .SetEase(Ease.InOutSine);
        sequence.AppendInterval(trailDelay);
        sequence.Append(healthBarTrailingFillImage.DOFillAmount(ratio, 0.3f))
            .SetEase(Ease.InOutSine);

        sequence.Play();
    }

    private void FlashWhite() {
        if (spriteRenderer != null) {
            Debug.Log("Flashing white");

            // Cancel any existing color tweens so they don’t stack
            spriteRenderer.DOKill();

            // Flash to white, then back to original color
            Color originalColor = spriteRenderer.color;
            spriteRenderer.DOColor(Color.red, 0.1f).OnComplete(() =>
                spriteRenderer.DOColor(originalColor, 0.3f)
            );
        }
    }

    private void Shake() {
        transform.DOKill();

        float shakeAngle = 10f; // how far it rotates in degrees
        float duration = 0.2f;  // total duration of the shake

        // rotate a bit clockwise, then counterclockwise, then reset
        Sequence seq = DOTween.Sequence();
        seq.Append(transform.DORotate(new Vector3(0, 0, shakeAngle), duration * 0.25f).SetEase(Ease.OutQuad));
        seq.Append(transform.DORotate(new Vector3(0, 0, -shakeAngle), duration * 0.5f).SetEase(Ease.InOutQuad));
        seq.Append(transform.DORotate(Vector3.zero, duration * 0.25f).SetEase(Ease.OutQuad));
        seq.Play();
    }
}
