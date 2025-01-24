using UnityEngine;

public class Breakable : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // V�rifie si l'objet en collision est le joueur
        PlayerController player = collision.GetComponent<PlayerController>();

        if (player != null)
        {
            Debug.Log($"Le joueur {collision.gameObject.name} est touch� par {gameObject.name}.");

            // Passe la variable MORT du joueur � true
            player.isDead = true;

            // Facultatif : D�truire cet objet apr�s avoir affect� le joueur
            Destroy(gameObject);

            // Facultatif : Jouer une animation ou un effet sonore
        }
    }
}
