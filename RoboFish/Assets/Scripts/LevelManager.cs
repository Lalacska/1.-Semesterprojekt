using TMPro;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField]
    GameObject completedCanvas;

    [SerializeField]
    TimerScript timerScript;

    [SerializeField]
    TimeSaveManager timeSaveManager;

    [SerializeField]
    GameObject completedTimeObject;
    TextMeshProUGUI completedTimeText;

    private void Awake()
    {
        completedTimeText = completedTimeObject.GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        if(NewRoboController.Instance.isWinning == true && FishController.Instance.isWinning == true)
        {
            Time.timeScale = 0;
            Win();
        }
    }
    private void Start()
    {
        Time.timeScale = 1;
    }

    void Win()
    {
        string usedTime = timerScript.UsedTime();
        timeSaveManager.SaveTime(usedTime);
        Debug.Log("You have won!");
        completedCanvas.SetActive(true);
        completedTimeText.text = usedTime;
    }

    
}
