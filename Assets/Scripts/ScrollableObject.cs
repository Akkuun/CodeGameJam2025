using UnityEngine;

public class ScrollableObject : MonoBehaviour
{
    private ScrollManager speedManager;
    private Vector2 initialPosition;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        speedManager = ScrollManager.instance;
        speedManager.resetDistance();
        initialPosition = transform.position;// on met la position initiale 
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

    public void AdjustPosition(float offset)
    {
        // Ajuste la position en ajoutant un décalage
        transform.position = new Vector3(transform.position.x - offset, transform.position.y, transform.position.z);
    }
}
