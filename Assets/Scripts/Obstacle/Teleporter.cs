using System.Threading.Tasks;
using UnityEngine;
using System.Collections;
using System;


public class Teleporter : MonoBehaviour
{
    [Header("Destination de téléportation")]
    public Transform teleportDestination; // Point où le joueur sera téléporté
    public ScrollManager scrollManager;
    [Header("Effets et paramètres")]
    public bool useTeleportEffect = false; // Active un effet visuel lors de la téléportation
    public ParticleSystem teleportEffect; // Effet visuel de téléportation

    private ArrayList arrayListTheme = new ArrayList
        {ThemeManager.Theme.Modern, ThemeManager.Theme.Medieval, ThemeManager.Theme.Futuristic};

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

    private async void TeleportPlayer(GameObject player)
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



        FindObjectOfType<BloomTransitor>().m_shouldGlow = true;
        await Task.Delay(1000);
        ThemeManager themeManager = FindObjectOfType<ThemeManager>();
        ThemeManager.Theme currentTheme = themeManager.currentTheme;


        // Créer une liste des thèmes restants (en excluant le thème actuel)
        ArrayList availableThemes = new ArrayList(arrayListTheme);
        availableThemes.Remove(currentTheme);

        // Générer un thème aléatoire parmi les thèmes restants
        System.Random random = new System.Random();
        int randomIndex = random.Next(availableThemes.Count);
        ThemeManager.Theme randomTheme = (ThemeManager.Theme)availableThemes[randomIndex];

        // Appliquer le nouveau thème
        themeManager.ChangeTheme(randomTheme);

        // Désactiver l'effet Glow après le changement de thème
        FindObjectOfType<BloomTransitor>().m_shouldGlow = false;



        Debug.Log($"Joueur téléporté à {teleportDestination.position} avec un décalage de {offset}");
    }

}
