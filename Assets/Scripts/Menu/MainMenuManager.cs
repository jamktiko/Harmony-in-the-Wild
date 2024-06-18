using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;

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
        SaveManager.instance.DeleteSave();

        //reset the abilities again
        foreach (Abilities ability in Enum.GetValues(typeof(Abilities)))
        {
            AbilityManager.instance.abilityStatuses[ability] = false;
        }

        //reset the quests again
        //yes this is stupid. blame Awake()
        QuestManager.instance.questMap = QuestManager.instance.CreateQuestMap();
        if (QuestManager.instance.transform.childCount>0)
        {
            for (int i = 0; i < QuestManager.instance.transform.childCount; i++)
            {
                Destroy(QuestManager.instance.transform.GetChild(i).gameObject);
            }
        }
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
            //Debug.Log("changed yes");
        }
        else
        {
            PlayerPrefs.SetInt("InvertY", 1);
            //Debug.Log("changed no");
        }
    }
    public void CreditsButton() 
    {
        SceneManager.LoadScene(CreditsSceneName);
    }
}
