using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField]
    GameObject go;

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
        Debug.Log("You have won!");
        go.SetActive(true);
    }
}
