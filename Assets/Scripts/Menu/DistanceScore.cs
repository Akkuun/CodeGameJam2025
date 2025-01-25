using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;


public class DistanceScore : MonoBehaviour
{
    
    public TMP_Text distanceText;
    public float distance;
    private PlayerController player;

    public string medievalFontPath = "Fonts/medievalFont"; // Police médiévale
    public string futuristicFontPath = "Fonts/futuristicFont"; // Police futuriste

    private TMP_FontAsset medievalFont;
    private TMP_FontAsset futuristicFont;


    void Start()
    {
        medievalFont = Resources.Load<TMP_FontAsset>(medievalFontPath);
        futuristicFont = Resources.Load<TMP_FontAsset>(futuristicFontPath);

        if (medievalFont == null || futuristicFont == null)
        {
            Debug.LogError("Les polices n'ont pas été trouvées. Vérifiez les chemins dans le dossier Resources !");
        }

        UpdateDistanceText(); 
    }

    void Update()
    {
       
        UpdateDistanceText();
    }

    void UpdateDistanceText()
    {
        if (ScrollManager.instance != null)
        {
            //distance = ScrollManager.instance.distanceScrolled;
            //distanceText.text = Mathf.FloorToInt(distance).ToString() + " m";

            GameObject playerObject = GameObject.FindWithTag("Player");

            if (playerObject != null)
            {
                player = playerObject.GetComponent<PlayerController>();
                if (player != null)
                {

                    //Debug.Log(player.levelPosition);

                    if (player.levelPosition == 0)
                    {
                        distanceText.font = medievalFont; // Change la police en médiévale
                    }
                    else if (player.levelPosition == 1)
                    {
                        distanceText.font = futuristicFont; // Change la police en futuriste
                    }

                    distanceText.text = player.score.ToString() + " m";


                }
            }
        }
    }

    public void ResetDistance()
    {
        distance = 0f;
        UpdateDistanceText();
    }
}
