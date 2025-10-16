using UnityEngine;

public class EnemyPathing : MonoBehaviour {
    [SerializeField] private Transform target;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float wiggleSpeed = 5f;
    [SerializeField] private float wiggleAngle = 10f;

    private bool reachedTarget = false;
    private float baseRotation;
    private float wiggleOffset;

    private void OnEnable() {
        reachedTarget = false;
        wiggleOffset = Random.Range(0f, 2f * Mathf.PI);
    }

    private void Update() {
        if (target == null) return;

        if (!reachedTarget) {
            transform.position = Vector2.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);

            if (Vector2.Distance(transform.position, target.position) < 3f) {
                reachedTarget = true;
                baseRotation = transform.eulerAngles.z;
            }
        }
        else {
            // Wiggle slightly in place
            float angle = Mathf.Sin(Time.time * wiggleSpeed + wiggleOffset) * wiggleAngle;
            transform.rotation = Quaternion.Euler(0f, 0f, baseRotation + angle);
        }
    }
}
