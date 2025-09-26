using UnityEngine;

public class RockCollision : MonoBehaviour
{
    [Header("Cam Shake Settings")]
    [SerializeField] private float shakeDuration = 0.1f;
    [SerializeField] private float shakeMagnitude = 0.1f;

    [SerializeField] private Rigidbody2D playerRb;
    [SerializeField] private float killJumpForce = 5f;

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("Enemy")) {
            CameraShake.Instance.Shake(shakeDuration, shakeMagnitude);

            Vector2 entryDir = (transform.position - collision.transform.position).normalized;

            playerRb.linearVelocity = Vector2.zero;
            playerRb.AddForce(entryDir * killJumpForce, ForceMode2D.Impulse);

            ObjectPoolManager.ReturnObjectToPool(collision.gameObject, ObjectPoolManager.PoolType.GameObjects);
        }
    }
}
