using UnityEngine;

public class LandingBurst : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private ParticleSystem landingParticles;

    private bool wasGrounded = false;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if we hit a collider with tag "Platform"
        if (collision.collider.CompareTag("Platform"))
        {
            foreach (ContactPoint2D contact in collision.contacts)
            {
                SoundManager.PlaySound(SoundType.Robot_Land);
                // contact.normal points *out* of the collider we hit
                // for floor collisions, normal.y should be positive (pointing up)
                if (contact.normal.y > 0.5f)
                {
                    if (landingParticles != null)
                        landingParticles.Play();
                    break; // only need one valid contact
                }
            }
        }
    }
}
