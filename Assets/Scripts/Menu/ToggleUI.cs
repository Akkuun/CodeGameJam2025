using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using System;
using System.Collections;
using UnityEngine;


public class ToggleUI : MonoBehaviour
{
    [SerializeField] private GameObject textsPanel; 
    [SerializeField] private ScrollManager gameManager;
    private KeyCode toggleKey = KeyCode.UpArrow; // Jump to start game and remove UI text

    private bool isVisible = true;

    void Start()
    {
        gameManager = ScrollManager.instance;
    }

    void Update()
    {
        // If focus is on Input text 
        if (EventSystem.current.currentSelectedGameObject != null &&
            (EventSystem.current.currentSelectedGameObject.GetComponent<InputField>() != null ||
             EventSystem.current.currentSelectedGameObject.GetComponent<TMP_InputField>() != null))
        {
            return;
        }

        if (Input.GetKeyDown(toggleKey))
        {

            gameManager.gameState = GameState.Tutorial;
            isVisible = false;
            textsPanel.SetActive(isVisible);
        }
    }
}
