using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public Transform pointA;
    public Transform pointB;
    public float movespeed = 2;
    public float interval;
    private float timer;
    public bool sticky = false;

    private Vector3 nextPosition;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        timer = interval;
        nextPosition = pointB.position;

    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, nextPosition, movespeed * Time.deltaTime);
        if (transform.position == nextPosition)
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                nextPosition = (nextPosition == pointA.position) ? pointB.position : pointA.position;
                timer = interval;
            }
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
}
