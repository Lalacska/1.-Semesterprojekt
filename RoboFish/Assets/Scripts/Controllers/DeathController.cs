using UnityEngine;

public class DeathController : MonoBehaviour
{
    [SerializeField]
    NewRoboController roboController;

    [SerializeField]
    FishController fishController;

    public bool dieTogether = false;
    private bool oldValue;

    private void OnValidate()
    {
        if (dieTogether != oldValue)
        {
            oldValue = dieTogether;
            fishController.SetDieTogehter(dieTogether);
            roboController.SetDieTogehter(dieTogether);
        }
    }
}
