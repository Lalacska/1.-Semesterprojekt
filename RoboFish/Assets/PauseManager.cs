using UnityEngine;
using UnityEngine.InputSystem;
using PauseMenu;

public class PauseManager : MonoBehaviour
{
    [SerializeField] PauseMenu.PauseMenu PauseMenu;
    InputAction openPause;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        openPause = InputSystem.actions.FindAction("Pause");
    }

    // Update is called once per frame
    void Update()
    {
        if (openPause.WasPressedThisFrame())
        {
            PauseMenu.Pause();
        }
    }
}
