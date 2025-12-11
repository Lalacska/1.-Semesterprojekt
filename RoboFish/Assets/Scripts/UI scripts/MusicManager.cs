using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{
    public AudioMixerGroup musicMixer;

    public AudioSource melodySource;
    public AudioSource drumsSource;


    public bool enableDrums;

    private void Awake()
    {
        if (melodySource.isPlaying)
        {
            melodySource.Play();
        }

        if(SceneManager.GetActiveScene().buildIndex == 0)
        {
            enableDrums = false;
        }

        UpdateDrums();
    }

    public void Update()
    {
        if (Time.timeScale == 0)
        {
            if (drumsSource.isPlaying)
            {
                drumsSource.Pause();
            }
        }
        else
        {
            UpdateDrums();
        }

        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            enableDrums = false;
        }
        else
        {
            enableDrums = true;
        }
    }



    public void UpdateDrums() //tjekker og sikrer at drums og melodi er in time
    {
        if(enableDrums == true)
        {
            if (!drumsSource.isPlaying)
            {
                drumsSource.timeSamples = melodySource.timeSamples;
                drumsSource.Play();
            }
        }
        else
        {
            if (enableDrums == false)
            {

                if (drumsSource.isPlaying)
                {
                    drumsSource.Pause();
                }
            }
        }
    }



    public void EnableDrums()
    {
        enableDrums = true;
        UpdateDrums();
    }

    public void DisableDrums()
    {
        enableDrums = false;
        UpdateDrums();
    }


}
