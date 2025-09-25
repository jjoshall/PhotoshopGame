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

    private Vector2 dragStartPos;
    private bool isDragging = false;

    // Getters
    public Vector2 DragStartPos => dragStartPos;
    public bool IsDragging => isDragging;
    public float MaxLaunchForce => maxLaunchForce;
    public float ForceMultiplier => forceMultiplier;
    public Rigidbody2D Rb => rb;

    public void Shoot(InputAction.CallbackContext context) {
        if (context.started) {
            Debug.Log("Mouse Down");

            // Record the position where the drag started
            dragStartPos = mainCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            isDragging = true;
        }
        else if (context.canceled && isDragging) {
            Debug.Log("Mouse Up");

            // Record mouse up position
            Vector2 dragEndPos = mainCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue());

            // Calculate drag vector
            Vector2 dragVector = dragEndPos - dragStartPos;

            // Opposite direction for launch
            Vector2 launchDir = -dragVector;

            // Clamp the launch force
            Vector2 clampedForce = Vector2.ClampMagnitude(launchDir * forceMultiplier, maxLaunchForce);

            // Apply force to the Rigidbody2D
            rb.AddForce(clampedForce, ForceMode2D.Impulse);

            isDragging = false;
        }
    }
}
