using System;
using TMPro;
using UnityEngine;

public class TimerScript : MonoBehaviour
{
    
    public TMP_Text TimerText;

    public GameObject gameOverPanel;

    public float totalTime; // Total time in seconds

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        totalTime -= Time.deltaTime; // Decrease total time by the time elapsed since last frame
        float minutes = Mathf.FloorToInt(totalTime / 60); // Get the minutes part
        float seconds = Mathf.FloorToInt(totalTime % 60); // Get the seconds part
        TimerText.text = $"0{minutes}:{seconds}";

        if (totalTime <= 0)
        {
            gameOverPanel.SetActive(true);
            TimerText.text = "00:00";
            
        }
    }
}
