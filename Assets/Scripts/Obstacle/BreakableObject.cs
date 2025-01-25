using UnityEngine;

public class Breakable : MonoBehaviour
{
    [SerializeField] private AudioSource breakSound;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // V�rifie si l'objet en collision est le joueur
        PlayerController player = collision.gameObject.GetComponent<PlayerController>();

        if (player != null)
        {
            // Si le joueur est en glissade, ne pas d�clencher la mort
            if (player.isSliding)
            {
                // D�truire l'objet si le joueur glisse
                breakSound.Play();
                Destroy(gameObject);
            }
            else
            {
                // Passe la variable isDead � true si le joueur n'est pas en glissade
                Debug.Log($"Le joueur {collision.gameObject.name} est touch� par {gameObject.name}.");
                //player.isDead = true;

                // Facultatif : D�truire l'objet apr�s la mort
                //Destroy(gameObject);
            }
        }
    }
}
