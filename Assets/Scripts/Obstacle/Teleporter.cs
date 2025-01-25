using UnityEngine;

public class Teleporter : MonoBehaviour
{
    [Header("Destination de t�l�portation")]
    public Transform teleportDestination; // Point o� le joueur sera t�l�port�
    public ScrollManager scrollManager;
    [Header("Effets et param�tres")]
    public bool useTeleportEffect = false; // Active un effet visuel lors de la t�l�portation
    public ParticleSystem teleportEffect; // Effet visuel de t�l�portation
    public NoteSpawner noteSpawner;


    void Start()
    {
        scrollManager = ScrollManager.instance;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        // V�rifie si l'objet entrant est le joueur
        if (collision.GetComponent<PlayerController>()!=null)
        {
            // T�l�portation
            TeleportPlayer(collision.gameObject);
        }
    }

    private void TeleportPlayer(GameObject player)
    {
        if (teleportDestination == null)
        {
            Debug.LogError("Aucune destination de t�l�portation d�finie !");
            return;
        }

        // Sauvegarde l'ancienne position du joueur
        Vector3 oldPosition = player.transform.position;

        // Si un effet de t�l�portation est activ�
        if (useTeleportEffect && teleportEffect != null)
        {
            Instantiate(teleportEffect, oldPosition, Quaternion.identity); // Effet avant la t�l�portation
        }

        // T�l�porte le joueur
        //player.transform.position = teleportDestination.position;

        // Calcule l'offset (distance entre l'ancienne et la nouvelle position)
        float offset = teleportDestination.position.x - oldPosition.x;

        // Ajuste les objets d�filants via ScrollManager
        scrollManager.AdjustObjectsAfterTeleport(offset);

        // Effet apr�s la t�l�portation
        if (useTeleportEffect && teleportEffect != null)
        {
            Instantiate(teleportEffect, teleportDestination.position, Quaternion.identity);
        }
        noteSpawner.destroyNotes();
        noteSpawner.spawnNote();
        Debug.Log($"Joueur t�l�port� � {teleportDestination.position} avec un d�calage de {offset}");
    }

}
