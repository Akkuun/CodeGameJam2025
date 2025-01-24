using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DistanceScore : MonoBehaviour
{
    
    public Text distanceText;
    private float distance = 0f;
    public float speed = 1f; 

    void Start()
    {
        UpdateDistanceText(); 
    }

    void Update()
    {
        distance += speed * Time.deltaTime;

        UpdateDistanceText();
    }

    void UpdateDistanceText()
    {
        distanceText.text = Mathf.FloorToInt(distance).ToString() + " m";
    }

    public void ResetDistance()
    {
        distance = 0f;
        UpdateDistanceText();
    }
}
