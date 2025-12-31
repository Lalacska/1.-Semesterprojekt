using System;
using UnityEngine;

public enum SoundType
{
    Fish_Move,
    Robot_Walk,
    Robot_Jump,
    Robot_Land,
    Robot_Death,
    Lever,
    Gate,
    Platform,
    UI_Click
}

[RequireComponent(typeof(AudioSource)), ExecuteInEditMode]
public class SoundManager : MonoBehaviour
{
    [SerializeField] private SoundList[] soundList;
    private static SoundManager instance;
    private AudioSource audioSource;

    private void Awake()
    {
        instance = this;

        //SoundManager[] managers =
        //    UnityEngine.Object.FindObjectsByType<SoundManager>(
        //        FindObjectsInactive.Include,
        //        FindObjectsSortMode.None
        //    );

        //if (managers.Length >= 2)
        //{
        //    Destroy(gameObject);
        //    return;
        //}

        //DontDestroyOnLoad(gameObject);
    }


    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public static void PlaySound(SoundType sound, float  volume = 1)
    {
        AudioClip[] clips = instance.soundList[(int)sound].Sounds;
        AudioClip randomClip = clips[UnityEngine.Random.Range(0, clips.Length)];
        instance.audioSource.PlayOneShot(randomClip, volume);
    }

    public static AudioClip GetRandomClip(SoundType sound)
    {
        AudioClip[] clips = instance.soundList[(int)sound].Sounds;
        return clips[UnityEngine.Random.Range(0, clips.Length)];
    }

#if UNITY_EDITOR
    private void OnEnable()
    {
        string[] names = Enum.GetNames(typeof(SoundType));
        Array.Resize(ref soundList, names.Length );
        for (int i = 0; i < soundList.Length; i++) {
            soundList[i].name = names[i];
        }
    }

#endif

}

[Serializable]
public struct SoundList
{
    public AudioClip[] Sounds { get => sounds; }
    [HideInInspector] public string name;
    [SerializeField] private AudioClip[] sounds;
}
