using UnityEngine;

public class LevelManager : MonoBehaviour
{


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(NewRoboController.Instance.isWinning == true && FishController.Instance.isWinning == true)
        {
            Win();
        }
    }


    void Win()
    {
        Debug.Log("Congrats you won");
    }
}
