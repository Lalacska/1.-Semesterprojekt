using System;
using TMPro;
using UnityEngine;



namespace Timer { 
    public class TimerScript : MonoBehaviour
    {

        public TMP_Text TimerText;

        public GameObject gameOverPanel;

        public float totalTime; // Total time in seconds

        float startTime;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            startTime = totalTime;
        }

        // Update is called once per frame
        void Update()
        {

            totalTime -= Time.deltaTime; // Decrease total time by the time elapsed since last frame
            float minutes = Mathf.FloorToInt(totalTime / 60); // Get the minutes part
            float seconds = Mathf.FloorToInt(totalTime % 60); // Get the seconds part
            if (seconds < 10)
            {
                TimerText.text = $"0{minutes}:0{seconds}";

            }
            else
            {
                TimerText.text = $"0{minutes}:{seconds}";

            }

            if (totalTime <= 0)
            {
                gameOverPanel.SetActive(true);
                TimerText.text = "00:00";

            }
        }

        public string UsedTime()
        {
            float usedTime = startTime - totalTime;
            float minutes = Mathf.FloorToInt(usedTime / 60); // Get the minutes part
            float seconds = Mathf.FloorToInt(usedTime % 60); // Get the seconds part
            if (seconds < 10)
            {
                return $"0{minutes}:0{seconds}";

            }
            else
            {
                return $"0{minutes}:{seconds}";

            }

        }
    }
}
