using System;
using UnityEngine;

public class TimeSaveManager : MonoBehaviour
{
    [SerializeField]
    string levelName;

    public void SaveTime(string time)
    {
        bool isBetter = BetterTime(time);
        if(isBetter)
            PlayerPrefs.SetString(levelName, time);
    }

    public bool BetterTime(string time)
    {
        string oldTime = PlayerPrefs.GetString(levelName, "00:00");
        int oldTimeInInt;

        if (!string.IsNullOrEmpty(oldTime) && oldTime != "00:00")
        {
            oldTimeInInt = TimeStringToSeconds(oldTime, "oldtime");
        }
        else
        {
            oldTimeInInt = 0;
        }

        int newTimeInInt = TimeStringToSeconds(time, "newtime");
        Debug.Log(oldTime);
        if (oldTimeInInt > newTimeInInt || oldTime == "00:00")
        {
            Debug.Log("Converted");
            return true;
        }
        else
        {
            return false;
        }
    }

    public static int TimeStringToSeconds(string timeString, string name)
    {
        Debug.Log(name + ": " + timeString);
        string[] parts = timeString.Split(':');
        foreach (string part in parts)
        {
            Debug.Log(part);
        }
        int minutes = int.Parse(parts[0]);
        int seconds = int.Parse(parts[1]);
        return minutes * 60 + seconds;
    }
}
