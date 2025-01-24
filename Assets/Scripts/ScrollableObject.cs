using UnityEngine;

public class ScrollableObject : MonoBehaviour
{
    private SpeedManager speedManager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        speedManager = SpeedManager.instance;
    }

    // Update is called once per frame
    public void Update()
    {
        transform.position += Vector3.left * speedManager.GetSpeed() * Time.deltaTime;
        /*
        if (transform.position.x <= destroyX)
        {
            Destroy(gameObject);
        }
        */
    }
}
