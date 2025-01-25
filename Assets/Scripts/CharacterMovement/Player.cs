using UnityEngine;

public class Player : MonoBehaviour
{
    private int score = 0; // Le score actuel du joueur

    void Start()
    {
    }

    void Update()
    {
        UpdateScore(); 
    }

    void UpdateScore()
    {
        //distanceText.text = Mathf.FloorToInt(distance).ToString() + " m";
        if (ScrollManager.instance != null)
        {
            score = Mathf.FloorToInt(ScrollManager.instance.distanceScrolled);
        }
    }

    public int getScore() {  return score; }

    public void ResetScore()
    {
        score = 0;
        Debug.Log("Le score a été réinitialisé.");
    }
}
