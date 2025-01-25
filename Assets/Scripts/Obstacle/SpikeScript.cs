using UnityEngine;

public class SpikeScript : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // V�rifie si l'objet en collision est le joueur
        PlayerController player = collision.GetComponent<PlayerController>();

        if (player != null)
        {
            // Passe la variable isDead � true si le joueur n'est pas en glissade
            Debug.Log($"Le joueur {collision.gameObject.name} est touch� par {gameObject.name}.");
            player.isDead = true;
        }
    }
}
