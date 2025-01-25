using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using System;
using System.Collections;
using UnityEngine;


public class ToggleGameOverUI : MonoBehaviour
{
    [SerializeField] private GameObject textsPanel; 
    private KeyCode toggleKey = KeyCode.UpArrow; // Jump to start game and remove UI text

    private bool isVisible = true;
    private TMP_Text scoreText;

    void Start()
    {
    }

    void Update()
    {
        scoreText.text = ""+PlayerPrefs.GetInt("score");
        // If focus is on Input text 
        if (EventSystem.current.currentSelectedGameObject != null &&
            (EventSystem.current.currentSelectedGameObject.GetComponent<InputField>() != null ||
             EventSystem.current.currentSelectedGameObject.GetComponent<TMP_InputField>() != null))
        {
            return;
        }

        if (Input.GetKeyDown(toggleKey))
        {

            // gameManager.gameState = GameState.Title;
            // isVisible = false;
            // textsPanel.SetActive(isVisible);
            
            // Load the main scene
            UnityEngine.SceneManagement.SceneManager.LoadScene("GameRelease");
        }
    }
}
