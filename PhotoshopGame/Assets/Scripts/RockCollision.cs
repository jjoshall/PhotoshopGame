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

    [SerializeField] private Rigidbody2D playerRb;
    [SerializeField] private float killJumpForce = 5f;

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("Enemy")) {
            score += scorePerEnemy;
            scoreNum.text = score.ToString();

            DeathTimer.Instance.AddTime();

            CameraShake.Instance.Shake(shakeDuration, shakeMagnitude);
            HitStop.Instance.Stop(hitStopDuration);

            Vector2 entryDir = (transform.position - collision.transform.position).normalized;

            playerRb.linearVelocity = Vector2.zero;
            playerRb.AddForce(entryDir * killJumpForce, ForceMode2D.Impulse);

            ObjectPoolManager.ReturnObjectToPool(collision.gameObject, ObjectPoolManager.PoolType.GameObjects);
        }
    }
}
