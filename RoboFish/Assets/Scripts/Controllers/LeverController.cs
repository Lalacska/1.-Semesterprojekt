using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class LeverController : MonoBehaviour, IInteractable
{
    //[SerializeField] GameObject gate;
    //public GatePlatform gateScript;

    [SerializeField]   
    List<GatePlatform> Gates;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //gateScript = gate.GetComponent<GatePlatform>();
    }

    public void Interact()
    {
        Debug.Log("Lever");
        foreach (var gate in Gates) {
            gate.Interact();
        }
    }
}
