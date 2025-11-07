using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class RoboController : MonoBehaviour
{
    Rigidbody2D rb;
    string currentScene;

    float movementX = 0;
    float movementY = 0;

    public float maxSpeed = 10;
    public float speed = 10;
    public float jumpForce = 7;
    public bool onGround;
    bool jump = false;
    LeverController currentLever;

    //Lever controlls
    public InputAction actionInput;
    private IInteractable interactTarget;
    private bool rightShiftPressed = false;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentScene = SceneManager.GetActiveScene().name;
    }

    // Update is called once per frame
    void Update()
    {
        // Reset flag when the key is released
        if (Keyboard.current.enterKey.wasReleasedThisFrame)
        {
            rightShiftPressed = false;
        }
        if (Input.GetKeyDown(KeyCode.Space) && onGround == true)
        {
            jump = true;
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

        if (jump)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            jump = false;
        }

    }

    private void OnAction()
    {
        // Prevent multiple triggers if the key is held
        if (!rightShiftPressed)
        {
            rightShiftPressed = true;
            Debug.Log("Robot");
            interactTarget?.Interact();
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
            interactTarget = collision.GetComponent<IInteractable>();
            Debug.Log(currentLever);
            Debug.Log(interactTarget);
        }

        if (collision.gameObject.CompareTag("Water"))
        {
            SceneManager.LoadScene(currentScene);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Lever"))
        {
            this.currentLever = null;
        }
        if (collision.GetComponent<IInteractable>() == interactTarget)
            interactTarget = null;
    }

    void OnMove(InputValue movementValue)
    {

        // Convert the input value into a Vector2 for movement.
        Vector2 input = movementValue.Get<Vector2>();

        // Store the X and Y components of the movement.
        movementX = input.x;
        movementY = input.y;
    }


}
