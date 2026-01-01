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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

#if (UNITY_EDITOR)
        if (resetTimes)
        {
            TimeReset();
        }
#endif
        TimeSetting();

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
            levelName = level.name;
            TimeText.text = "00:00";
            PlayerPrefs.SetString(levelName, $"00:00");
        }
    }




}
