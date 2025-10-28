using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class GateScript : MonoBehaviour
{
    public bool open = false;
    
    public float distance = 0.64f;
    public float speed = 0.005f;
    private Vector3 startposition;
    public int direction = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Vector3 startposition = transform.position;
    }
        

    

    // Update is called once per frame
    void FixedUpdate()
    {
        switch(direction) {
            case(0): 
                if (open)
                {
                    if (transform.position.y < startposition.y + distance)
                    {
                        transform.Translate(0f, speed, 0f);
                        break;
                    }
                }
                else
                {
                    if (transform.position.y > startposition.y)
                    {
                        transform.Translate(0f, -speed, 0f);
                        break;
                    }
                }break;
            case (1):
                if (open)
                {
                    if (transform.position.x < startposition.x + distance)
                    {
                        transform.Translate(speed, 0f, 0f);
                        break;
                    }
                }
                else
                {
                    if (transform.position.x > startposition.x)
                    {
                        transform.Translate(-speed, 0f, 0f);
                        break;
                    }
                }
                break;
            case (2):
                if (open)
                {
                    if (transform.position.y > startposition.y - distance)
                    {
                        transform.Translate(0f, -speed, 0f);
                        break;
                    }
                }
                else
                {
                    if (transform.position.y < startposition.y)
                    {
                        transform.Translate(0f, speed, 0f);
                        break;
                    }
                }
                break;
                case (3):
                if (open)
                {
                    if (transform.position.x > startposition.x - distance)
                    {
                        transform.Translate(-speed, 0f, 0f);
                        break;
                    }
                }
                else
                {
                    if (transform.position.x < startposition.x)
                    {
                        transform.Translate(speed, 0f, 0f);
                        break;
                    }
                }break;
        }
        

    }
    
}
