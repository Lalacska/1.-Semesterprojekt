using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using System.Collections;

public class FishController : MonoBehaviour
{
    public static FishController Instance { get; set; }
    Rigidbody2D rb;

    Vector3 originalPos;
    float movementX = 0;
    float movementY = 0;
    public bool inWater = false; // <-- track if fish is in water
    bool dieTogether = false;
    bool _isFacingRight;


    [SerializeField] private float moveForce = 10f;
    [SerializeField] private float jumpForce = 2f;
    [SerializeField] private float moveForceWater = 200f;
    [SerializeField] private float waterDrag = 4f;
    [SerializeField] private float airDrag = 0.5f;
    [SerializeField] private float maxSpeed = 5f;
    [SerializeField] private float sinkingForce = 5f;
    [SerializeField] private Animator _animator;

    [SerializeField] public GameObject failedCanvas;
    //Lever controlls
    LeverController currentLever;
    private IInteractable interactTarget;
    private bool actionButtonPressed = false;

    //Win Condition
    public bool isWinning = false;

    public bool isGroundedInWater = false;
    private bool swimming;

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
        if (inWater && rb.linearVelocity.magnitude > 0.1f)
        {
            swimming = true;
            _animator.SetBool("anim_swimming", true);
        }
        else
        {
            swimming = false;
            _animator.SetBool("anim_swimming", false);
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

            if (input.sqrMagnitude > 0.001f || !isGroundedInWater)
            {
                rb.AddForce(velocityChange * moveForceWater * Time.fixedDeltaTime, ForceMode2D.Force);
            }
            else if (isGroundedInWater)
            {
                // HARD STOP when resting
                rb.linearVelocity = Vector2.zero;
            }

            // Gentle sinking when not pressing up
            if (movementY <= 0.01f && !isGroundedInWater)
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


        // Particle handling
        if (part == null) return;

        ParticleSystem.EmissionModule emission = part.emission;

        bool shouldEmit =
            inWater &&
            !isGroundedInWater &&
            rb.linearVelocity.magnitude > 0.1f;

        emission.enabled = shouldEmit;

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Water"))
        {
            rb.gravityScale = 0f;
            inWater = true;

            rb.linearVelocity *= 0.5f;
            rb.linearDamping = waterDrag;
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
        _animator.SetBool("anim_dead", true);
        StartCoroutine(DeathAnimation());
    }
    IEnumerator DeathAnimation()
    {
        if (dieTogether)
        {
            _animator.SetBool("anim_dead", true);
            yield return new WaitForSeconds(0.7f);
            _animator.SetBool("anim_dead", false);
            //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            failedCanvas.SetActive(true);
            Time.timeScale = 0;
        }
        else
        {
            _animator.SetBool("anim_dead", true);
            yield return new WaitForSeconds(0.7f);
            _animator.SetBool("anim_dead", false);
            transform.position = originalPos;
        }
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Platform") && inWater)
        {
            if (CheckGrounded())
            {
                isGroundedInWater = true;
            }
        }

        if (collision.gameObject.CompareTag("Finish"))
        {
            isWinning = true;
        }
    }

    // Simple ground check using a small raycast
    bool CheckGrounded()
    {
        CircleCollider2D col = GetComponent<CircleCollider2D>();
        if (col == null) return false;

        // start a little above the bottom of the circle
        Vector2 origin = new Vector2(transform.position.x, transform.position.y - col.radius + 0.01f);
        float distance = 0.05f; // how far below to check
        LayerMask groundMask = LayerMask.GetMask("Ground");

        RaycastHit2D hit = Physics2D.Raycast(origin, Vector2.down, distance, groundMask);
        Debug.DrawRay(origin, Vector2.down * distance, Color.red); // visualize in Scene view

        return hit.collider != null;
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Platform") && inWater)
        {
            isGroundedInWater = false;
        }

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
        TurnCheck(movementX, movementY);

    }

    // This function is called when action button is pushed
    void OnAction()
    {
        if (!actionButtonPressed)
        {
            actionButtonPressed = true;
            Debug.Log("Hey you pressed E");
            interactTarget?.Interact();
        }
    }

    void TurnCheck(float directionX, float directionY)
    {

        if (directionX > 0)
        {
            _isFacingRight = true;
            if (directionY > 0)
            {
                transform.rotation = Quaternion.Euler(0, 0, 20);
            }
            else if (directionY < 0)
            {
                transform.rotation = Quaternion.Euler(0, 0, -20);
            }
            else
            {
                transform.rotation = Quaternion.Euler(0, 0, 0);
            }
        }
        else if (directionX < 0)
        {
            _isFacingRight = false;
            if (directionY > 0)
            {
                transform.rotation = Quaternion.Euler(0, 180, 20);
            }
            else if (directionY < 0)
            {
                transform.rotation = Quaternion.Euler(0, 180, -20);
            }
            else
            {
                transform.rotation = Quaternion.Euler(0, 180, 0);
            }
        }
        else if (directionX == 0)
        {
            if (directionY > 0)
            {
                if (_isFacingRight)
                    transform.rotation = Quaternion.Euler(0, 0, 20);
                else
                    transform.rotation = Quaternion.Euler(0, 180, 20);
            }
            else if (directionY < 0)
            {
                if (_isFacingRight)
                    transform.rotation = Quaternion.Euler(0, 0, -20);
                else
                    transform.rotation = Quaternion.Euler(0, 180, -20);
            }
            else
            {
                if (_isFacingRight)
                    transform.rotation = Quaternion.Euler(0, 0, 0);
                else
                    transform.rotation = Quaternion.Euler(0, 180, 0);
            }
        }
    }
}



