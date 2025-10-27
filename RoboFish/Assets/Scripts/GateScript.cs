using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class GateScript : MonoBehaviour
{
    public bool open = false;
    public int direction = 0;
    public float distance = -0.64f;
    public float speed = 0.005f;
    public Vector3 startposition;
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
                if (!open)
                {
                    if (transform.position.y < startposition.y)
                    {
                        transform.Translate(0f, speed, 0f);
                        break;
                    }
                }
                else
                {
                    if (transform.position.y > startposition.y + distance)
                    {
                        transform.Translate(0f, -speed, 0f);
                        break;
                    }
                }
                    break;
        }
        

    }
    
}
