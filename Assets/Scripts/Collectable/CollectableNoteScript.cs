using UnityEngine;

public class Collectable : MonoBehaviour
{
    private ScrollManager gameManager;
    void Start()
    {
        gameManager = ScrollManager.instance;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // V�rifie si l'objet en collision est le joueur
        PlayerController player = collision.GetComponent<PlayerController>();

        if (player != null)
        {
            
        }
    }
}
