using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LoadSavedTime : MonoBehaviour
{

    [SerializeField]
    List<GameObject> levels;

    [SerializeField]
    bool resetTimes = false;

    string levelName;

    const int RESET_VERSION = 1; // Increment this whenever you want to force a reset

    void Awake()
    {
        // Check if the player has already gone through this reset
        if (PlayerPrefs.GetInt("TimeResetVersion", 0) < RESET_VERSION)
        {
            TimeReset(); // Run the reset once
            PlayerPrefs.SetInt("TimeResetVersion", RESET_VERSION); // Mark it as done
            PlayerPrefs.Save();
        }

        TimeSetting(); // Load the times (new or old)
    }

    // Goes trough the levels to load the saved time for each of them
    public void TimeSetting()
    {
        foreach (var level in levels)
        {
            TMP_Text TimeText = level.GetComponentInChildren<TMP_Text>();
            Debug.Log(TimeText);
            levelName = level.name;
            LoadTime(TimeText);
        }
    }


    // Finds the time and loads it in to the right text element
    public void LoadTime(TMP_Text TimeText)
    {
        if (levelName != null)
        {

            Debug.Log(levelName);
            string time = PlayerPrefs.GetString(levelName);
            if (PlayerPrefs.HasKey(levelName) && !string.IsNullOrEmpty(time))
            {
                TimeText.text = time;

            }
            else
            {
                TimeText.text = "00:00";
                PlayerPrefs.SetString(levelName, $"00:00");
            }
        }
    }

    // Reset times for every level
    public void TimeReset()
    {
        foreach (var level in levels)
        {
            TMP_Text TimeText = level.GetComponentInChildren<TMP_Text>();
            string templevelName = level.name;
            TimeText.text = "00:00";
            Debug.Log(templevelName);
            PlayerPrefs.SetString(templevelName, $"00:00");
        }

        PlayerPrefs.Save();
    }




}
