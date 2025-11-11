using UnityEngine;

[CreateAssetMenu(menuName ="Robot Movement")]
public class RoboMovementStats : ScriptableObject
{
    [Header("Walk")]
    [Range(1f, 100f)] public float maxWalkSpeed = 12.5f;
    [Range(0.25f, 50f)] public float groundAcceleration = 5f;
    [Range(0.25f, 50f)] public float groundDeceleration = 20f;
    [Range(0.25f, 50f)] public float airAcceleration = 5f;
    [Range(0.25f, 50f)] public float airDeceleration = 5f;

    [Header("Grounded/Collision Checks")]
    public LayerMask groundLayer;
    public float groundDetectionRayLength = 0.02f;
    public float headDetectionRayLength = 0.02f;
    [Range(0f, 1f)] public float headWidth = 0.75f;

    [Header("Jump")]
    public float jumpHeight = 0.8f;
    [Range(1f, 1.1f)] public float jumpHeighCompensationFactor = 1.054f;
    public float timeTillJumpApex = 0.5f;
    [Range(0.01f, 5f)] public float gravityOnReleaseMultiplier = 2f;
    public float maxFallSpeed = 3f;
    [Range(1, 5)] public int numberOfJumpsAllowed = 1;

    [Header("Jump Cut")]
    [Range(0.02f, 0.3f)] public float timeForUpwardsCancel = 0.027f;

    [Header("Jump Apex")]
    [Range(0.5f, 1f)] public float apexThreshold = 0.97f;
    [Range(0.01f, 1f)] public float apexHangTime = 0.075f;

    [Header("Jump Buffer")]
    [Range(0f, 1f)] public float jumpBufferTime = 0.125f;

    [Header("Jump Coyote Time")]
    [Range(0f, 1f)] public float jumpCoyoteTime = 0.1f;

    [Header("Debug")]
    public bool debugShowIsGroundedBox;
    public bool debugShowHeadBumpBox;

    [Header("JumpVisualisation Tool")]
    public bool showWalkJumpArc = false;
    public bool stopOnCollision = true;
    public bool drawRight = true;
    [Range(5, 100)] public int arcResolution = 20;
    [Range(0, 500)] public int visualizationSteps = 90;

    public float gravity { get; private set; }
    public float initialJumpVelocity { get; private set; }

    public float adjustedJumpHeight { get; private set; }


    private void OnValidate()
    {
        CalculateValues();
    }
    private void OnEnable()
    {
        CalculateValues();
    }

    void CalculateValues()
    {
        adjustedJumpHeight = jumpHeight * jumpHeighCompensationFactor;
        gravity = -(2f * adjustedJumpHeight) / Mathf.Pow(timeTillJumpApex, 2f);
        initialJumpVelocity = Mathf.Abs(gravity) * timeTillJumpApex;
    }
}

