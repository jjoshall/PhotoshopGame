using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Physics Movement Settings")]
    [SerializeField] private float maxLaunchForce = 15f;
    [SerializeField] private float forceMultiplier = 1f;

    [Header("References")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private GameObject dashPs;

    [Header("Time Slow Settings")]
    [SerializeField] private float timeSlowFactor = 0.1f; // Factor to slow down time while dragging
    [SerializeField] private float holdDuration = 0.5f; // How long to stay fully slowed
    [SerializeField] private float recoveryDuration = 2f; // How long to lerp back

    private Vector2 dragStartPos;
    private bool isDragging = false;
    private float timeSlowTimer = 0f;
    private bool isTimeSlowed = false;

    // Getters
    public Vector2 DragStartPos => dragStartPos;
    public bool IsDragging => isDragging;
    public float MaxLaunchForce => maxLaunchForce;
    public float ForceMultiplier => forceMultiplier;
    public Rigidbody2D Rb => rb;

    private void Update() {
        if (!isTimeSlowed) return;

        timeSlowTimer += Time.unscaledDeltaTime;

        if (timeSlowTimer < holdDuration) {
            Time.timeScale = timeSlowFactor;
        }
        else if (timeSlowTimer < holdDuration + recoveryDuration) {
            float t = (timeSlowTimer - holdDuration) / recoveryDuration;
            Time.timeScale = Mathf.Lerp(timeSlowFactor, 1f, t);
        }
        else {
            Time.timeScale = 1f;
            isTimeSlowed = false;
            timeSlowTimer = 0f;
        }
    }

    public void Shoot(InputAction.CallbackContext context) {
        if (context.started) {
            // Record the position where the drag started
            dragStartPos = mainCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            isDragging = true;

            // Slow time while dragging
            isTimeSlowed = true;
        }
        else if (context.canceled && isDragging) {
            Time.timeScale = 1f; // Reset time scale
            isTimeSlowed = false;
            timeSlowTimer = 0f; // Reset timer

            // Record mouse up position
            Vector2 dragEndPos = mainCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue());

            // Calculate drag vector
            Vector2 dragVector = dragEndPos - dragStartPos;

            // Opposite direction for launch
            Vector2 launchDir = -dragVector;

            // Clamp the launch force
            Vector2 clampedForce = Vector2.ClampMagnitude(launchDir * forceMultiplier, maxLaunchForce);

            // Reset velo so new shot overrides old shot
            rb.linearVelocity = Vector2.zero;

            // Get angle for ps
            float angle = Mathf.Atan2(launchDir.y, launchDir.x) * Mathf.Rad2Deg;

            angle += 180f;

            Quaternion particleRotation = Quaternion.Euler(-angle, 90f, 0f);

            ObjectPoolManager.SpawnObject(
                dashPs, 
                transform.position, 
                particleRotation, 
                ObjectPoolManager.PoolType.ParticleSystems
            );

            // Apply force to the Rigidbody2D
            rb.AddForce(clampedForce, ForceMode2D.Impulse);

            isDragging = false;
        }
    }
}
