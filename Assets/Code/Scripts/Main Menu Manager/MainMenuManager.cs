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
        // check if tutorial is still in progress
        if(QuestManager.instance.CheckQuestState("Tutorial") != QuestState.FINISHED)
        {
            // check the current quest step state to see if there's still something to be done in Bear Cave
            if(QuestManager.instance.GetQuestById("Tutorial").GetCurrentQuestStepIndex() < 4)
            {
                GameEventsManager.instance.uiEvents.ShowLoadingScreen(SceneManagerHelper.Scene.Tutorial);
            }

            // otherwise transfer to Overworld so the quest can be finished there
            else
            {
                GameEventsManager.instance.uiEvents.ShowLoadingScreen(SceneManagerHelper.Scene.Overworld_VS);
            }
        }

        // if tutorial has been finished, go to Overworld
        else
        {
            GameEventsManager.instance.uiEvents.ShowLoadingScreen(SceneManagerHelper.Scene.Overworld_VS);
        }      
    }

    private void CheckSavedGame()
    {
        if (!continueButton.IsInteractable() && File.Exists(Application.persistentDataPath + "/GameData.json"))
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
        QuestManager.instance.CheckAllRequirements();

        /*if (QuestManager.instance.transform.childCount > 0)
        {
            for (int i = 0; i < QuestManager.instance.transform.childCount; i++)
            {
                Destroy(QuestManager.instance.transform.GetChild(i).gameObject);
            }
        }*/

        GameEventsManager.instance.uiEvents.ShowLoadingScreen(SceneManagerHelper.Scene.Storybook);
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

    public void ChangeYInversion(bool isOn)
    {
        if (isOn)
        {
            PlayerPrefs.SetInt("InvertY", 1);
            //Debug.Log("changed yes");
        }
        else
        {
            PlayerPrefs.SetInt("InvertY", 0);
            //Debug.Log("changed no");
        }
    }
    public void CreditsButton()
    {
        SceneManagerHelper.LoadScene(SceneManagerHelper.Scene.Credits);
    }

    public void DiscordButton() 
    {
        Application.OpenURL("https://discord.gg/7jwSSEn22M");
    }
}
