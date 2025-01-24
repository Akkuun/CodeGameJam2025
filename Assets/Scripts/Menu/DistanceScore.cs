using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;


public class DistanceScore : MonoBehaviour
{
    
    public TMP_Text distanceText;
    public float distance;
    private PlayerController player;


    void Start()
    {
        UpdateDistanceText(); 
    }

    void Update()
    {
        //distance += speed * Time.deltaTime;

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
