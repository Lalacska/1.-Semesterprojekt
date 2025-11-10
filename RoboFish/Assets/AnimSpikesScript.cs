using UnityEngine;

public class AnimSpikesScript : MonoBehaviour
{
    [SerializeField] private Animator anim;
    private const string RAISE_PARAM = "Raise";
    [SerializeField] private float interval = 3f; // hvor ofte spikes aktiveres

    private float timer;

    void Start()
    {
        timer = interval; // starter timeren
        if (anim == null)
            anim = GetComponent<Animator>();
    }

    void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            anim.SetTrigger(RAISE_PARAM); // trigger animationen
            timer = interval; // nulstil timeren
        }
    }
}
