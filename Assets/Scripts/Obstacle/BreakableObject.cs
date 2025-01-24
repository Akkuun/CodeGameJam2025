using UnityEngine;

public class Breakable : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Vérifie si l'objet en collision est le joueur
        PlayerController player = collision.GetComponent<PlayerController>();

        if (player != null)
        {
            Debug.Log($"Le joueur {collision.gameObject.name} est touché par {gameObject.name}.");

            // Passe la variable MORT du joueur à true
            player.isDead = true;

            // Facultatif : Détruire cet objet après avoir affecté le joueur
            Destroy(gameObject);

            // Facultatif : Jouer une animation ou un effet sonore
        }
    }
}
