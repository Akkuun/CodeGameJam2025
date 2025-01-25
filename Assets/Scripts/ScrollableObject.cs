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
        switch (speedManager.gameState)
        {
            case GameState.Title:
                break;
            case GameState.Tutorial:
            case GameState.Game:
            case GameState.SegmentTransition:
                Scroll();
                break;
            case GameState.GameOver:
                break;
        }
        
        /*
        if (transform.position.x <= destroyX)
        {
            Destroy(gameObject);
        }
        */
    }

    public void Scroll() {
        transform.position += Vector3.left * speedManager.GetSpeed() * Time.deltaTime;
    }
    
    public void AdjustPosition(float offset)
    {
        // Ajuste la position en ajoutant un décalage
        transform.position = new Vector3(transform.position.x - offset, transform.position.y, transform.position.z);
    }
}
