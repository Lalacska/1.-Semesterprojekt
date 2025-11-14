using UnityEngine;

public class WaterController : MonoBehaviour
{
    public Transform pointA;
    public Transform pointB;
    public float movespeed = 0.01f;
    
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, pointB.position, movespeed * Time.deltaTime);
    }

    private void FixedUpdate()
    {
        //scaleY += risingSpeed;
        //gb.transform.position = new Vector3(gb.transform.localScale.x, scaleY, gb.transform.localScale.z);
        ////gb.transform.localScale = new Vector3(gb.transform.localScale.x, scaleY, gb.transform.localScale.z);
    }
}
