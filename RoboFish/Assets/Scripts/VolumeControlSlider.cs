using UnityEngine;
using UnityEngine.UI;

public class VolumeControlSlider : MonoBehaviour
{
    public Slider volumeSlider;
    public AudioSource audioSource;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        audioSource.volume = volumeSlider.value;
    }
}
