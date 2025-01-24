using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using System;
using System.Collections;
using UnityEngine;


public class ToggleUI : MonoBehaviour
{
    [SerializeField] private GameObject textsPanel; 
    private KeyCode toggleKey = KeyCode.UpArrow; // Jump to start game and remove UI text

    private bool isVisible = true;

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
            isVisible = false;
            textsPanel.SetActive(isVisible);
        }
    }
}
