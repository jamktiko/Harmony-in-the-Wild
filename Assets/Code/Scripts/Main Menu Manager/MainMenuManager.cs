using System;
using System.IO;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    public const string CreditsSceneName = "Credits";

    [FormerlySerializedAs("options")] [SerializeField] private GameObject _options;
    [FormerlySerializedAs("mainMenu")] [SerializeField] private GameObject _mainMenu;
    [FormerlySerializedAs("settings")] [SerializeField] private GameObject _settings;
    [FormerlySerializedAs("gameplayControls")] [SerializeField] private GameObject _gameplayControls;
    [FormerlySerializedAs("movementControls")] [SerializeField] private GameObject _movementControls;
    [FormerlySerializedAs("invertYAxis")] [SerializeField] private Toggle _invertYAxis;

    [FormerlySerializedAs("playButtonSceneName")] [SerializeField] private string _playButtonSceneName; //TODO: don't rely on strings in inspector
    [FormerlySerializedAs("continueButton")] [SerializeField] private Button _continueButton;
    [FormerlySerializedAs("controlsMenu")] [SerializeField] private GameObject _controlsMenu;

    private void Start()
    {
        if (PlayerPrefs.GetInt("InvertY") == 1)
        {
            _invertYAxis.isOn = true;
        }
        else
        {
            _invertYAxis.isOn = false;
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
        if (QuestManager.Instance.CheckQuestState("Tutorial") != QuestState.Finished)
        {
            // check the current quest step state to see if there's still something to be done in Bear Cave
            if (QuestManager.Instance.GetQuestById("Tutorial").GetCurrentQuestStepIndex() < 4)
            {
                GameEventsManager.instance.UIEvents.ShowLoadingScreen(SceneManagerHelper.Scene.Tutorial);
            }

            // otherwise transfer to Overworld so the quest can be finished there
            else
            {
                GameEventsManager.instance.UIEvents.ShowLoadingScreen(SceneManagerHelper.Scene.Overworld);
            }
        }

        // if tutorial has been finished, go to Overworld
        else
        {
            GameEventsManager.instance.UIEvents.ShowLoadingScreen(SceneManagerHelper.Scene.Overworld);
        }
    }

    private void CheckSavedGame()
    {
        if (!_continueButton.IsInteractable() && File.Exists(Application.persistentDataPath + "/GameData.json"))
        {
            _continueButton.interactable = true;
        }

    }

    public void StartNewGame()
    {
        SaveManager.Instance.DeleteSave();

        //reset the abilities again
        foreach (Abilities ability in Enum.GetValues(typeof(Abilities)))
        {
            AbilityManager.Instance.AbilityStatuses[ability] = false;
        }

        //reset the quests again
        //yes this is stupid. blame Awake()
        QuestManager.Instance.QuestMap = QuestManager.Instance.CreateQuestMap();
        QuestManager.Instance.CheckAllRequirements();

        /*if (QuestManager.instance.transform.childCount > 0)
        {
            for (int i = 0; i < QuestManager.instance.transform.childCount; i++)
            {
                Destroy(QuestManager.instance.transform.GetChild(i).gameObject);
            }
        }*/

        GameEventsManager.instance.UIEvents.ShowLoadingScreen(SceneManagerHelper.Scene.Storybook);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void Options()
    {
        _options.SetActive(true);
        _mainMenu.SetActive(false);
    }

    public void BackButton()
    {
        _mainMenu.SetActive(true);
        _options.SetActive(false);
        _settings.SetActive(false);
        _movementControls.SetActive(false);
        _gameplayControls.SetActive(false);
    }

    public void SettingsButton()
    {
        _options.SetActive(false);
        _settings.SetActive(true);
    }

    public void MovementControlsButton()
    {
        _settings.SetActive(false);
        _movementControls.SetActive(true);
    }

    public void ControlsButton()
    {
        _controlsMenu.SetActive(true);
        _options.SetActive(false);
    }

    public void GameplayControlsButton()
    {
        _settings.SetActive(false);
        _gameplayControls.SetActive(true);
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
