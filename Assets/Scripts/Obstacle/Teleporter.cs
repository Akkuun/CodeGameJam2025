using System.Threading.Tasks;
using UnityEngine;
using System.Collections;
using System;


public class Teleporter : MonoBehaviour
{
    [Header("Destination de t�l�portation")]
    public Transform teleportDestination; // Point o� le joueur sera t�l�port�
    public ScrollManager scrollManager;
    [Header("Effets et param�tres")]
    public bool useTeleportEffect = false; // Active un effet visuel lors de la t�l�portation
    public ParticleSystem teleportEffect; // Effet visuel de t�l�portation

    private ArrayList arrayListTheme = new ArrayList
        {ThemeManager.Theme.Modern, ThemeManager.Theme.Medieval, ThemeManager.Theme.Futuristic};

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

    private async void TeleportPlayer(GameObject player)
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



        FindObjectOfType<BloomTransitor>().m_shouldGlow = true;
        await Task.Delay(1000);
        ThemeManager themeManager = FindObjectOfType<ThemeManager>();
        ThemeManager.Theme currentTheme = themeManager.currentTheme;


        // Cr�er une liste des th�mes restants (en excluant le th�me actuel)
        ArrayList availableThemes = new ArrayList(arrayListTheme);
        availableThemes.Remove(currentTheme);

        // G�n�rer un th�me al�atoire parmi les th�mes restants
        System.Random random = new System.Random();
        int randomIndex = random.Next(availableThemes.Count);
        ThemeManager.Theme randomTheme = (ThemeManager.Theme)availableThemes[randomIndex];

        // Appliquer le nouveau th�me
        themeManager.ChangeTheme(randomTheme);

        // D�sactiver l'effet Glow apr�s le changement de th�me
        FindObjectOfType<BloomTransitor>().m_shouldGlow = false;



        Debug.Log($"Joueur t�l�port� � {teleportDestination.position} avec un d�calage de {offset}");
    }

}
