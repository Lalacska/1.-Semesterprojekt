using NUnit.Framework;
using System.Collections.Generic;
using System.Dynamic;
using UnityEngine;

public class LeverController : MonoBehaviour, IInteractable
{
    //[SerializeField] GameObject gate;
    //public GatePlatform gateScript;

    [SerializeField]   
    List<GatePlatform> Gates;
    [SerializeField] private Animator _animator;
    public bool isFlipped = false;
    public int color;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _animator.SetInteger("Color", color);
        //gateScript = gate.GetComponent<GatePlatform>();
    }
    void Update()
    {
      
    }

    public void Interact()
    {
        if (isFlipped)
        {
            isFlipped = false;
            _animator.SetBool("isFlipped", false);

        }
        else
        {
            isFlipped = true;
            _animator.SetBool("isFlipped", true);
        }
        Debug.Log("Lever Flipped");
        foreach (var gate in Gates) {
            gate.Interact();
        }
    }
}
