using UnityEngine;
using UnityEngine.InputSystem;

public class RoboInputManager : MonoBehaviour
{
    public static PlayerInput playerInput;

    public static Vector2 movement;
    public static bool jumpWasPressed;
    public static bool jumpIsHeld;
    public static bool jumpIsReleased;

    private InputAction _moveAction;
    private InputAction _jumpAction;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();

        _moveAction = playerInput.actions["Move"];
        _jumpAction = playerInput.actions["Jump"];
    }

    private void Update()
    {
        movement = _moveAction.ReadValue<Vector2>();

        jumpWasPressed = _jumpAction.WasPressedThisFrame();
        jumpIsHeld = _jumpAction.IsPressed();
        jumpIsReleased = _jumpAction.WasReleasedThisFrame();
    }
}
