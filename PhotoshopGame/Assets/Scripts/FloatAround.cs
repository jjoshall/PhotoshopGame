using UnityEngine;

public class FloatAround : MonoBehaviour {
    public float moveSpeed = 0.5f;
    public float rotationSpeed = 30f;
    public float directionChangeTime = 2f;

    private Vector2 moveDirection;
    private float directionTimer;
    private float rotationTimer;
    private bool isRotating;

    void OnEnable() {
        Reset();
    }

    void Update() {
        // Move
        transform.Translate(moveDirection * moveSpeed * Time.deltaTime, Space.World);

        // Occasionally rotate
        if (isRotating)
            transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);

        // Timers
        directionTimer -= Time.deltaTime;
        rotationTimer -= Time.deltaTime;

        // Pick new direction
        if (directionTimer <= 0f) {
            PickNewDirection();
            directionTimer = directionChangeTime + Random.Range(-1f, 1f);
        }

        // Toggle rotation
        if (rotationTimer <= 0f) {
            isRotating = !isRotating;
            rotationTimer = Random.Range(1f, 3f);
        }
    }

    void Reset() {
        // Called whenever the object is enabled (spawned from pool)
        PickNewDirection();
        directionTimer = directionChangeTime;
        rotationTimer = Random.Range(1f, 3f);
        isRotating = Random.value > 0.5f;

        // Optional: reset rotation so reused ones don’t keep old spins
        transform.rotation = Quaternion.identity;
    }

    void PickNewDirection() {
        moveDirection = Random.insideUnitCircle.normalized;
    }
}
