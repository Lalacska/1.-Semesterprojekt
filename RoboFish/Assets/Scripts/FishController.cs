using UnityEngine;
using UnityEngine.InputSystem;

public class FishController : MonoBehaviour
{
    Rigidbody2D rb;

    float movementX = 0;
    float movementY = 0;
    private bool inWater = false; // <-- track if fish is in water

    [SerializeField] private float moveForce = 10f;
    [SerializeField] private float jumpForce = 2f;
    [SerializeField] private float waterDrag = 1f;
    [SerializeField] private float airDrag = 0.5f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }


    private void FixedUpdate()
    {
        if (inWater)
        {
            // Create a 2D movement vector using the X and Y inputs.
            Vector2 movement = new Vector2(movementX, movementY);

            // Apply force to the Rigidbody to move the player.
            rb.AddForce( movement * moveForce, ForceMode2D.Force);

            // Apply gentle sinking when not pressing up
            if (movementY <= 0.01f)
            {
                rb.AddForce(Vector2.down * (moveForce * 0.1f), ForceMode2D.Force);
            }

            // Optional: Clamp velocity to prevent runaway speed underwater
            rb.linearVelocity = Vector2.ClampMagnitude(rb.linearVelocity, 5f);
        }
        else
        {
            Vector2 movement = new Vector2(movementX, 0f);
            rb.AddForce(movement * moveForce);

        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Water")) 
        {
            Debug.Log("Water");
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("a");
        if (other.CompareTag("Water"))
        {
            GetComponent<Rigidbody2D>().gravityScale = 0f;
            inWater = true;

            rb.linearVelocity *= 0.5f;

            rb.linearDamping = waterDrag;
        }
        if (other.CompareTag("Platform") && !other.CompareTag("Water"))
        {
            Debug.Log("I died?");
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        Debug.Log("b");
        if (other.CompareTag("Water"))
        {
            GetComponent<Rigidbody2D>().gravityScale = 1f;
            inWater = false;

            rb.linearDamping = airDrag;

            // If moving upward fast, apply jump boost
            if (rb.linearVelocity.y > 2f)
            {
                rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            }
        }
    }

    // This function is called when a move input is detected.
    void OnMove(InputValue movementValue)
    {
        
        // Convert the input value into a Vector2 for movement.
        Vector2 input = movementValue.Get<Vector2>();

        // Store the X and Y components of the movement.
        movementX = input.x;
        movementY = input.y;
    }

    void OnAction()
    {
        Debug.Log("Hey you pressed E");
    }
}
