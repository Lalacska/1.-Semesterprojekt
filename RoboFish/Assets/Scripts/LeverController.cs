using UnityEngine;

public class LeverController : MonoBehaviour, IInteractable
{
    [SerializeField] GameObject gate;
    public GateScript gateScript;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gateScript = gate.GetComponent<GateScript>();
    }

    public void Interact()
    {

        Debug.Log("Lever");
        gateScript.Interact();
    }
}
