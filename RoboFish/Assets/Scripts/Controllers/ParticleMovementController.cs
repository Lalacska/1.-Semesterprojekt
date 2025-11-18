using UnityEngine;

public class ParticleMovementController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private ParticleSystem particle;
    [SerializeField] private Rigidbody2D rb;

    [Header("Settings")]
    public bool requireGrounded = false; // for dust trails
    public bool requireAirborne = false; // for bubbles/swimming
    public float minSpeed = 0.2f;

    private ParticleSystem.EmissionModule emission;

    void Start()
    {
        if (particle != null)
            emission = particle.emission;

        if (rb == null)
            rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (particle == null || rb == null)
            return;

        float speed = rb.linearVelocity.magnitude;

        bool meetsSpeed = speed > minSpeed;

        bool meetsGroundState = true;

        // These conditions only apply if enabled
        if (requireGrounded)
            meetsGroundState = CheckGrounded();
        if (requireAirborne)
            meetsGroundState = !CheckGrounded();

        // Final emission toggle
        emission.enabled = meetsSpeed && meetsGroundState;
    }

    // Simple ground check using a small raycast
    bool CheckGrounded()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 0.2f, LayerMask.GetMask("Ground"));
        return hit.collider != null;
    }
}
