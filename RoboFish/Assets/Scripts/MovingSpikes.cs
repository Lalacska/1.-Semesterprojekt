using System.Collections;
using UnityEngine;

public class MovingSpikes : MonoBehaviour
{
    public float offsetTime;
    public Transform pointA;
    public Transform pointB;
    public float movespeed = 2;
    public float interval;
    private float timer;
    private Vector3 nextPosition;

    private void Update()
    {
        StartCoroutine(movingSpikes());
    }


    IEnumerator movingSpikes()
    {
        yield return new WaitForSeconds(offsetTime);
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
        if (collision.gameObject.CompareTag("Player"))
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
