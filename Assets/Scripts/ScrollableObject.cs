using UnityEngine;

public class ScrollableObject : MonoBehaviour
{
    public float speed = 1.0f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    public void Update()
    {
        transform.position += Vector3.left * speed * Time.deltaTime;
        /*
        if (transform.position.x <= destroyX)
        {
            Destroy(gameObject);
        }
        */
    }
}
