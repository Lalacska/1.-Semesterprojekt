using Unity.VisualScripting;
using UnityEngine;

public enum SoundType
{
    ROBOTIDLE,
    ROBOTJUMP,
    ROBOTLAND,
    ROBOTDEATH,
    FISHIDLE,
    FISHSWIM,


}









public class SoundManager : ScriptableObject
{
    private static SoundManager instance;

    private void Awake()
    {
        
    }

    static readonly object padlock = new object();

    SoundManager()
    {
    }

    public static SoundManager Instance
    {
        get
        {
            lock (padlock)
            {
                if (instance == null)
                {
                    instance = new SoundManager();
                }
                return instance;
            }
        }
    }


}
