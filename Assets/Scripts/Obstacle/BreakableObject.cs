using UnityEngine;

public class Breakable : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Vérifie si l'objet en collision est le joueur
        PlayerController player = collision.GetComponent<PlayerController>();

        if (player != null)
        {
            // Si le joueur est en glissade, ne pas déclencher la mort
            if (player.isSliding)
            {
                // Détruire l'objet si le joueur glisse
                Destroy(gameObject);
            }
            else
            {
                // Passe la variable isDead à true si le joueur n'est pas en glissade
                Debug.Log($"Le joueur {collision.gameObject.name} est touché par {gameObject.name}.");
                player.isDead = true;

                // Facultatif : Détruire l'objet après la mort
                Destroy(gameObject);
            }
        }
    }
}
