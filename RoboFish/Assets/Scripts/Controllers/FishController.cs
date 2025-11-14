using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class FishController : MonoBehaviour
{
    public static FishController Instance { get; set; }
    Rigidbody2D rb;

    Vector3 originalPos;
    float movementX = 0;
    float movementY = 0;
    public bool inWater = false; // <-- track if fish is in water
    bool dieTogether = false;

    [SerializeField] private float moveForce = 10f;
    [SerializeField] private float jumpForce = 2f;
    [SerializeField] private float moveForceWater = 200f;
    [SerializeField] private float waterDrag = 4f;
    [SerializeField] private float airDrag = 0.5f;
    [SerializeField] private float maxSpeed = 5f;
    [SerializeField] private float sinkingForce = 5f;

    //Lever controlls
    LeverController currentLever;
    private IInteractable interactTarget;
    private bool actionButtonPressed = false;

    //Win Condition
    public bool isWinning = false;

    private void Awake()
    {
        isWinning = false;
        Instance = this;
    }

    //Particle controller
    [SerializeField]
    ParticleSystem part;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        originalPos = transform.position;
    }

    private void Update()
    {
        // Reset flag when the key is released
        if (Keyboard.current.eKey.wasReleasedThisFrame)
        {
            actionButtonPressed = false;
        }
    }

    private void FixedUpdate()
    {
        if (inWater)
        {
            Vector2 input = new Vector2(movementX, movementY);

            // Calculate the desired velocity
            Vector2 desiredVelocity = input * maxSpeed;

            // Calculate the difference between current velocity and desired velocity
            Vector2 velocityChange = desiredVelocity - rb.linearVelocity;

            // Apply only the needed force to move toward desired velocity
            rb.AddForce(velocityChange * moveForceWater * Time.fixedDeltaTime, ForceMode2D.Force);

            // Gentle sinking when not pressing up
            if (movementY <= 0.01f)
            {
                rb.AddForce(Vector2.down * sinkingForce, ForceMode2D.Force);
            }
        }
        else
        {
            Vector2 input = new Vector2(movementX, 0f);

            // Desired horizontal velocity in air
            Vector2 desiredVelocity = new Vector2(input.x * maxSpeed, rb.linearVelocity.y);

            Vector2 velocityChange = desiredVelocity - rb.linearVelocity;
            rb.AddForce(velocityChange * moveForce * Time.fixedDeltaTime, ForceMode2D.Force);
        }

        // Particle handling in FixedUpdate (after movement code)
        if (part != null && inWater)
        {
            var emission = part.emission;

            // Only emit if the fish is actually moving in water
            if (rb.velocity.magnitude > 0.1f)
            {
                emission.enabled = true;
            }
            else
            {
                emission.enabled = false;
            }
        }

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("a");
        if (other.CompareTag("Water"))
        {
            rb.gravityScale = 0f;
            inWater = true;

            rb.linearVelocity *= 0.5f;
            rb.linearDamping = waterDrag;
        }
        if (other.CompareTag("Platform") && !other.CompareTag("Water"))
        {
            Debug.Log("I died?");
        }
        if (other.CompareTag("SpikeDeath"))
        {
            Dead();
        }
        if (other.gameObject.CompareTag("Lever"))
        {
            this.currentLever = other.gameObject.GetComponent<LeverController>();
            interactTarget = other.GetComponent<IInteractable>();
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        Debug.Log("b");
        if (other.CompareTag("Water"))
        {
            rb.gravityScale = 1f;
            inWater = false;
            rb.linearDamping = airDrag;

            // If moving upward fast, apply jump boost
            if (rb.linearVelocity.y > 2f)
            {
                rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            }
        }
        if (other.gameObject.CompareTag("Lever"))
        {
            this.currentLever = null;
        }
        if (other.GetComponent<IInteractable>() == interactTarget)
            interactTarget = null;
    }

    public void SetDieTogehter(bool isTrue)
    {
        dieTogether = isTrue;
    }

    void Dead()
    {
        if (dieTogether)
        {
            //SceneManager.LoadScene("Test2");
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        else
        {
            transform.position = originalPos;
        }
    }
    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Finish"))
        {
            isWinning = true;
        }
    }
    public void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Finish"))
        {
            isWinning = false;
        }
    }
    // This function is called when a move input is detected.
    void OnMove(InputValue movementValue)
    {
        Debug.Log("Move!");
        // Convert the input value into a Vector2 for movement.
        Vector2 input = movementValue.Get<Vector2>();

        // Store the X and Y components of the movement.
        movementX = input.x;
        movementY = input.y;
    }

    void OnAction()
    {
        if (!actionButtonPressed)
        {
            actionButtonPressed = true;
            Debug.Log("Hey you pressed E");
            interactTarget?.Interact();
        }
    }
}
