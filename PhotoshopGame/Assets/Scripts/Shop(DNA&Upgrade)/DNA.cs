using TMPro;
using UnityEngine;

public class DNA : MonoBehaviour
{
    [SerializeField] private float spinSpeed = 30f;

    private void Update() {
        transform.Rotate(0f, 0f, spinSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("Player")) {
            // Play a sound

            DNACounter.Instance.AddDNA(1);

            ObjectPoolManager.ReturnObjectToPool(gameObject, ObjectPoolManager.PoolType.GameObjects);
        }
    }
}
