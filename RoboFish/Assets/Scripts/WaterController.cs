using UnityEngine;

public class WaterController : MonoBehaviour
{
    public float scaleY = 1;
    public float risingSpeed = 0.01f;
    GameObject gb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gb = gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        scaleY += risingSpeed;
        gb.transform.localScale = new Vector3(gb.transform.localScale.x, scaleY, gb.transform.localScale.z);
    }
}
