using UnityEngine;

public class TrajectoryLine : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private Transform player;
    [SerializeField] private Transform swordPivot;

    [Header("Trajectory Line Settings")]
    [SerializeField] private int segmentCount = 50;
    [SerializeField] private float segmentSpacing = 0.1f;
    [SerializeField] private float previewFraction = 0.25f; // only show first 25%

    [SerializeField] private float swordRotationOffset = 29f;

    private Camera mainCamera;
    private LineRenderer lineRenderer;

    private void Start() {
        // Grab line renderer component and set its number of points
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 0;
        lineRenderer.sortingOrder = -2;

        mainCamera = Camera.main;
    }

    private void Update() {
        if (playerMovement.IsDragging) {
            ShowTrajectory();
            RotateSwordToTrajectory();
        }
        else {
            lineRenderer.positionCount = 0;
        }
    }

    private void ShowTrajectory() {
        // Get drag vector (like PlayerMovement but not applied yet)
        Vector2 dragStart = playerMovement.DragStartPos;
        Vector2 dragCurrent = mainCamera.ScreenToWorldPoint(UnityEngine.InputSystem.Mouse.current.position.ReadValue());
        Vector2 dragVector = dragCurrent - dragStart;
        Vector2 launchDir = -dragVector;
        Vector2 initialVelocity = Vector2.ClampMagnitude(launchDir * playerMovement.ForceMultiplier, playerMovement.MaxLaunchForce);

        // Simulate points
        int pointsToShow = Mathf.CeilToInt(segmentCount * previewFraction);
        lineRenderer.positionCount = pointsToShow;

        Vector2 pos = player.position;
        Vector2 velocity = initialVelocity;
        float gravity = Physics2D.gravity.y * playerMovement.Rb.gravityScale;

        for (int i = 0; i < pointsToShow; i++) {
            lineRenderer.SetPosition(i, pos);
            pos += velocity * segmentSpacing;
            velocity.y += gravity * segmentSpacing; // apply gravity
        }
    }

    private void RotateSwordToTrajectory() {
        Vector2 dragStart = playerMovement.DragStartPos;
        Vector2 dragCurrent = mainCamera.ScreenToWorldPoint(UnityEngine.InputSystem.Mouse.current.position.ReadValue());
        Vector2 dragVector = dragCurrent - dragStart;

        Vector2 launchDir = -dragVector;

        float angle = Mathf.Atan2(launchDir.y, launchDir.x) * Mathf.Rad2Deg;

        swordPivot.rotation = Quaternion.Euler(0f, 0f, angle + swordRotationOffset);
    }
}
