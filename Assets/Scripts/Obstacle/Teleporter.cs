using UnityEngine;

public class Teleporter : MonoBehaviour
{
    [Header("Destination de téléportation")]
    public Transform teleportDestination; // Point où le joueur sera téléporté
    public ScrollManager scrollManager;
    [Header("Effets et paramètres")]
    public bool useTeleportEffect = false; // Active un effet visuel lors de la téléportation
    public ParticleSystem teleportEffect; // Effet visuel de téléportation


    void Start()
    {
        scrollManager = ScrollManager.instance;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Vérifie si l'objet entrant est le joueur
        if (collision.GetComponent<PlayerController>()!=null)
        {
            // Téléportation
            TeleportPlayer(collision.gameObject);
        }
    }

    private void TeleportPlayer(GameObject player)
    {
        if (teleportDestination == null)
        {
            Debug.LogError("Aucune destination de téléportation définie !");
            return;
        }

        // Sauvegarde l'ancienne position du joueur
        Vector3 oldPosition = player.transform.position;

        // Si un effet de téléportation est activé
        if (useTeleportEffect && teleportEffect != null)
        {
            Instantiate(teleportEffect, oldPosition, Quaternion.identity); // Effet avant la téléportation
        }

        // Téléporte le joueur
        //player.transform.position = teleportDestination.position;

        // Calcule l'offset (distance entre l'ancienne et la nouvelle position)
        float offset = teleportDestination.position.x - oldPosition.x;

        // Ajuste les objets défilants via ScrollManager
        scrollManager.AdjustObjectsAfterTeleport(offset);

        // Effet après la téléportation
        if (useTeleportEffect && teleportEffect != null)
        {
            Instantiate(teleportEffect, teleportDestination.position, Quaternion.identity);
        }

        Debug.Log($"Joueur téléporté à {teleportDestination.position} avec un décalage de {offset}");
    }

}
