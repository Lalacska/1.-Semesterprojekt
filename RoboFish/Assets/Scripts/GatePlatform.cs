using TMPro;
using UnityEngine;

public class GatePlatform : MonoBehaviour, IInteractable
{

    public Transform pointA;
    public Transform pointB;
    public float movespeed = 2;
    public bool sticky = false;
    private Vector3 nextPosition;

    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.loop = true;
        audioSource.spatialBlend = 0f; // 2D sound
    }

    void Start()
    {
        nextPosition = pointA.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, nextPosition, movespeed * Time.deltaTime);
        
        // Check if arrived
        if (transform.position == nextPosition)
        {
            StopSound();
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && sticky)
        {
            collision.gameObject.transform.parent = transform;
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.transform.parent = null;
        }
    }
    public void Activate()
    {
        PlaySound();
        nextPosition = (nextPosition == pointA.position) ? pointB.position : pointA.position;
    }

    public void Interact()
    {
        Activate();
    }

    public void PlaySound()
    {
        if (!audioSource.isPlaying)
        {
            audioSource.clip = SoundManager.GetRandomClip(SoundType.Gate);
            audioSource.Play();
        }
    }

    public void StopSound()
    {
        if (audioSource.isPlaying)
            audioSource.Stop();
    }
}
