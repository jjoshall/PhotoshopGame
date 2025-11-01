using TMPro;
using UnityEngine;

public class RockCollision : MonoBehaviour
{
    [Header("Cam Shake Settings")]
    [SerializeField] private float shakeDuration = 0.1f;
    [SerializeField] private float shakeMagnitude = 0.1f;

    [Header("Hit Stop Settings")]
    [SerializeField] private float hitStopDuration = 0.1f;

    [Header("Score Settings")]
    [SerializeField] private int scorePerEnemy = 1;
    [SerializeField] private TextMeshProUGUI scoreNum;
    private int score = 0;
    public int CurrentScore => score;

    [Header("Syringe Sprites")]
    [SerializeField] private int enemiesKilled = 0;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Sprite[] sprites;

    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private Rigidbody2D playerRb;
    [SerializeField] private float killJumpForce = 5f;
    [SerializeField] private float healthToAddAmt = 5f;

    [SerializeField] private GameObject enemyDeathPs;
    [SerializeField] private AudioClip[] enemyDeathSFX;

    [SerializeField] private GameObject dnaDrop;

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("Enemy")) {
            enemiesKilled++;
            SetSyringeSpriteState(enemiesKilled);

            score += scorePerEnemy;
            scoreNum.text = score.ToString();

            PlayerDeathBar.Instance.AddHealth(healthToAddAmt);

            CameraShake.Instance.Shake(shakeDuration, shakeMagnitude);
            HitStop.Instance.Stop(hitStopDuration);
            ObjectPoolManager.SpawnObject(enemyDeathPs, collision.transform.position, Quaternion.identity, ObjectPoolManager.PoolType.ParticleSystems);

            SoundEffectManager.Instance.PlayRandomSoundFXClip(enemyDeathSFX, collision.transform, 0.75f);

            ObjectPoolManager.SpawnObject(dnaDrop, collision.transform.position, Quaternion.identity, ObjectPoolManager.PoolType.GameObjects);

            Vector2 entryDir = (transform.position - collision.transform.position).normalized;

            playerRb.linearVelocity = Vector2.zero;
            playerRb.AddForce(entryDir * killJumpForce, ForceMode2D.Impulse);

            ObjectPoolManager.ReturnObjectToPool(collision.gameObject, ObjectPoolManager.PoolType.GameObjects);

            playerMovement.StopJumpSFX();
            playerMovement.ResetLaunchPermission();
        }
    }

    private void SetSyringeSpriteState(int killCount) {
        if (killCount == 0)
            spriteRenderer.sprite = sprites[0];
        else if (killCount == 1)
            spriteRenderer.sprite = sprites[1];
        else if (killCount == 2)
            spriteRenderer.sprite = sprites[2];
        else if (killCount == 3)
            spriteRenderer.sprite = sprites[3];
        else if (killCount == 4)
            spriteRenderer.sprite = sprites[4];
        else if (killCount == 5)
            spriteRenderer.sprite = sprites[5];
    }
}
