using UnityEngine;

public class ScrollableObject : MonoBehaviour
{
    private ScrollManager speedManager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        speedManager = ScrollManager.instance;
        speedManager.resetDistance();
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
}
