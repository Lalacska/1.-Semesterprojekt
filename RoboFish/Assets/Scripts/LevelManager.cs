using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField]
    GameObject go;

    [SerializeField]
    TimerScript timerScript;

    [SerializeField]
    TimeSaveManager timeSaveManager;


    // Update is called once per frame
    void Update()
    {
        if(NewRoboController.Instance.isWinning == true && FishController.Instance.isWinning == true)
        {
            Time.timeScale = 0;
            Win();
        }
    }


    void Win()
    {
        string usedTime = timerScript.UsedTime();
        timeSaveManager.SaveTime(usedTime);
        Debug.Log("You have won!");
        go.SetActive(true);
    }

    
}
