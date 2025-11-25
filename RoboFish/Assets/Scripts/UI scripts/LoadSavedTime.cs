using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LoadSavedTime : MonoBehaviour
{

    [SerializeField]
    List<GameObject> levels;

    string levelName;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        TimeSetting();
        //TimeReset();
    }

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

    public void TimeReset()
    {
        foreach (var level in levels)
        {
            TMP_Text TimeText = level.GetComponentInChildren<TMP_Text>();
            levelName = level.name;
            TimeText.text = "--:--";
            PlayerPrefs.SetString(levelName, $"00:00");
        }
    }




}
