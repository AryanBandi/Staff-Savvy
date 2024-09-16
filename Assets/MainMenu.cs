using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public Image trebleClef;
    public Image altoClef;
    public Image bassClef;
    public Image arrow;
    public Button switchClefs; // shuffle icon
    public Button left;
    public Button right;

    private int currentClefIndex = 0;
    private int currentAngle = 0;
    private Image[] clefs;
    private int key = 0;

    void Start()
    {
        // Initialize the array of clefs
        clefs = new Image[] { trebleClef, altoClef, bassClef };

        // Load the last used index, default to 0 if none saved
        currentClefIndex = PlayerPrefs.GetInt("ActiveClefIndex", 0);
        currentAngle = PlayerPrefs.GetInt("ActiveAngle", 0);
        UpdateClefVisibility();
        MoveArrow();
    }

    // Update which clef is visible based on the currentClefIndex
    void UpdateClefVisibility()
    {
        for (int i = 0; i < clefs.Length; i++)
        {
            clefs[i].gameObject.SetActive(i == currentClefIndex);
        }
    }

    void MoveArrow()
    {
        arrow.transform.rotation = Quaternion.Euler(0, 0, currentAngle);
        key = currentAngle / 30;
        PlayerPrefs.SetInt("Key", key);
    }

    // Method to shuffle between clefs
    public void SwitchClefs()
    {
        // Rotate through the clefs cyclically
        currentClefIndex = (currentClefIndex + 1) % clefs.Length;

        // Save the current index
        PlayerPrefs.SetInt("ActiveClefIndex", currentClefIndex);

        // Update the visibility of clefs
        UpdateClefVisibility();
    }

    public void OnRightButtonClick()
    {
        currentAngle = (currentAngle + 330) % 360;
        PlayerPrefs.SetInt("ActiveAngle", currentAngle);
        MoveArrow();
    }

    public void OnLeftButtonClick()
    {
        currentAngle = (currentAngle + 390) % 360;
        PlayerPrefs.SetInt("ActiveAngle", currentAngle);
        MoveArrow();
    }

    // Method to start the game
    public void PlayGame()
    {
        PlayerPrefs.SetInt("Key", key);
        SceneManager.LoadSceneAsync("Game");
    }
    
    // Method to exit
    public void exitGame()
    {
        Application.Quit();
    }
}