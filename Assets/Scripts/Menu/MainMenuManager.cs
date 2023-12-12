using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] GameObject options;
    [SerializeField] GameObject MainMenu;
    [SerializeField] GameObject settings;
    [SerializeField] GameObject gameplayControls;
    [SerializeField] GameObject movementControls;
    [SerializeField] Toggle InvertYAxis; 

    [SerializeField] string playButtonSceneName;

    private void Start()
    {
        if (PlayerPrefs.GetInt("InvertY") == 1)
        {
            InvertYAxis.isOn = true;
        }
        else
        {
            InvertYAxis.isOn = false;
        }
    }
    public void ContinueButton()
    {
        if (HasSavedGame())
        {
            LoadSavedGame();
        }
        else
        {
            Debug.Log("No saved game found.");
        }
    }

    private void LoadSavedGame()
    {
        throw new NotImplementedException();
    }

    private bool HasSavedGame()
    {
        throw new NotImplementedException();
    }

    public void StartNewGame() 
    {
        SceneManager.LoadScene(playButtonSceneName); 
    }
    public void ExitGame() 
    {
        Application.Quit();
    }
    public void Options() 
    {
        options.SetActive(true);
        MainMenu.SetActive(false);
    }
    public void BackButton() 
    {
        MainMenu.SetActive(true);
        options.SetActive(false);
        settings.SetActive(false);
        movementControls.SetActive(false);
        gameplayControls.SetActive(false);
    }
    public void SettingsButton()
    {
        options.SetActive(false);
        settings.SetActive(true);
    }
    public void MovementControlsButton()
    {
        settings.SetActive(false);
        movementControls.SetActive(true);
    }
    public void GameplayControlsButton()
    {
        settings.SetActive(false);
        gameplayControls.SetActive(true);
    }
    public void ChangeYInversion()
    {
        if (PlayerPrefs.GetInt("InvertY")==1)
        {
            PlayerPrefs.SetInt("InvertY", 0);
            Debug.Log("changed yes");
        }
        else
        {
            PlayerPrefs.SetInt("InvertY", 1);
            Debug.Log("changed no");
        }
    }
    public void CreditsButton() 
    {
        SceneManager.LoadScene("Credits");
    }

}
