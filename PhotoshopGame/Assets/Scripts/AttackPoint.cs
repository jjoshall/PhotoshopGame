using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class AttackPoint : MonoBehaviour
{
    public static AttackPoint Instance { get; private set; }

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
}
