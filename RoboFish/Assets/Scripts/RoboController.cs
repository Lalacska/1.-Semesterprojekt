using UnityEngine;
using UnityEngine.InputSystem;

public class RoboController : MonoBehaviour
{
    Rigidbody2D rb;


    float movementX = 0;
    float movementY = 0;

    public float maxSpeed = 10;
    public float speed = 10;
    public float jumpForce = 7;
    public bool onGround;
    bool jump = false;
    LeverController currentLever;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && onGround == true)
        {
            jump = true;
        }
        if (Input.GetKeyDown(KeyCode.F) && currentLever != null)
        {
            currentLever.FlipSwitch();
        }
    }
    private void FixedUpdate()
    {
        Vector2 movement = new Vector2(movementX, movementY);
        rb.AddForce(Vector2.right * movement * speed);

        if (rb.linearVelocity.x > maxSpeed)
        {
            rb.linearVelocity = new Vector2(maxSpeed, rb.linearVelocity.y);
        }
        if (rb.linearVelocity.x < -maxSpeed)
        {
            rb.linearVelocity = new Vector2(-maxSpeed, rb.linearVelocity.y);
        }

        //if (direction == 0)
        // {
        //     rb.linearVelocity = new Vector2 (0,rb.linearVelocity.y);
        // }

        if (jump)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            jump = false;
        }

    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Platform"))
        {
            onGround = true;
        }
        Debug.Log("Something");

    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Platform"))
        {
            onGround = false;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Lever"))
        {
            this.currentLever = collision.gameObject.GetComponent<LeverController>();
        }

        if (collision.gameObject.CompareTag("Water"))
        {
            Debug.Log("I'm Dead");
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Lever"))
        {
            this.currentLever = null;
        }
    }
    /*
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Lever") && Input.GetKeyDown(KeyCode.F))
        {
            collision.gameObject.GetComponent<LeverController>().FlipSwitch();
        }
    }
    */

    void OnMove(InputValue movementValue)
    {

        // Convert the input value into a Vector2 for movement.
        Vector2 input = movementValue.Get<Vector2>();

        // Store the X and Y components of the movement.
        movementX = input.x;
        movementY = input.y;
    }
}
