using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    public const string CreditsSceneName = "Credits";

    [SerializeField] private GameObject options;
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject settings;
    [SerializeField] private GameObject gameplayControls;
    [SerializeField] private GameObject movementControls;
    [SerializeField] private Toggle invertYAxis; 

    [SerializeField] private string playButtonSceneName; //TODO: don't rely on strings in inspector
    [SerializeField] private Button continueButton;
    [SerializeField] private GameObject controlsMenu;

    private void Start()
    {
        if (PlayerPrefs.GetInt("InvertY") == 1)
        {
            invertYAxis.isOn = true;
        }
        else
        {
            invertYAxis.isOn = false;
        }
        CheckSavedGame();
    }

    public void ContinueButton()
    {
            LoadSavedGame();
    }

    private void LoadSavedGame()
    {
        SceneManager.LoadScene(3);
    }

    private void CheckSavedGame()
    {
        if (!continueButton.IsInteractable()&& File.Exists(Application.persistentDataPath + "/GameData.json"))
        {
            continueButton.interactable = true;        
        }
        
    }

    public void StartNewGame() 
    {
        File.Delete(Application.persistentDataPath + "/GameData.json");
        Debug.LogError("The save file has been deleted. Please restart the game to avoid any errors.");
        SceneManager.LoadScene(playButtonSceneName); 
    }

    public void ExitGame() 
    {
        Application.Quit();
    }

    public void Options() 
    {
        options.SetActive(true);
        mainMenu.SetActive(false);
    }

    public void BackButton() 
    {
        mainMenu.SetActive(true);
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

    public void ControlsButton()
    {
        controlsMenu.SetActive(true);
        options.SetActive(false);
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
        SceneManager.LoadScene(CreditsSceneName);
    }
}
