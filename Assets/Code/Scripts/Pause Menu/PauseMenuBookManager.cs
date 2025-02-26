using Cinemachine;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class PauseMenuBookManager : MonoBehaviour
{
    [FormerlySerializedAs("pauseMenuPanel")] [SerializeField]
    public GameObject PauseMenuPanel;

    [FormerlySerializedAs("generalMenuPanel")] [SerializeField]
    public GameObject GeneralMenuPanel;

    [FormerlySerializedAs("optionsMenuPanel")] [SerializeField]
    public GameObject OptionsMenuPanel;

    [FormerlySerializedAs("movementControlsMenuPanel")] [SerializeField]
    public GameObject MovementControlsMenuPanel;

    [FormerlySerializedAs("questMenuPanel")] [SerializeField]
    public GameObject QuestMenuPanel;

    [FormerlySerializedAs("gamePlayControlsMenuPanel")] [SerializeField]
    public GameObject GamePlayControlsMenuPanel;

    [FormerlySerializedAs("settingsMenuPanel")] [SerializeField]
    public GameObject SettingsMenuPanel;

    [FormerlySerializedAs("activeQuestPanel")] [SerializeField]
    public GameObject ActiveQuestPanel;

    [FormerlySerializedAs("restartQuestPanel")] [SerializeField]
    public GameObject RestartQuestPanel;

    [FormerlySerializedAs("exitQuestPanel")] [SerializeField]
    public GameObject ExitQuestPanel;

    [FormerlySerializedAs("berryPanel")] [SerializeField]
    public GameObject BerryPanel;

    [FormerlySerializedAs("pineConePanel")] [SerializeField]
    public GameObject PineConePanel;

    [FormerlySerializedAs("cinemachineFreeLook")] [SerializeField] CinemachineFreeLook _cinemachineFreeLook;

    [FormerlySerializedAs("InvertYAxis")] [SerializeField]
    Toggle _invertYAxis;

    [FormerlySerializedAs("fullscreen")] [SerializeField]
    Toggle _fullscreen;

    [FormerlySerializedAs("Mastervolume")] [SerializeField]
    Slider _mastervolume;

    [FormerlySerializedAs("MusicVolume")] [SerializeField]
    Slider _musicVolume;

    [FormerlySerializedAs("sensitivity")] [SerializeField]
    Slider _sensitivity;

    [FormerlySerializedAs("mixer")] [SerializeField] AudioMixer _mixer;

    [FormerlySerializedAs("SliderValueMaster")] [SerializeField]
    private float _sliderValueMaster;

    [FormerlySerializedAs("SliderValueMusic")] [SerializeField]
    private float _sliderValueMusic;

    [FormerlySerializedAs("SliderValue2")] [SerializeField]
    private float _sliderValue2;

    [SerializeField]
    public TMP_Text BerryCounter,
                                     PineconeCounter;

    [FormerlySerializedAs("UIPanels")] [SerializeField] GameObject[] _uiPanels;
    [FormerlySerializedAs("activeQuestUI")] [SerializeField] ActiveQuestUI _activeQuestUI;

    private void Awake()
    {
        InitializeUIElementsList();
    }

    private void Update()
    {
        if (PlayerInputHandler.Instance.PauseInput.WasPressedThisFrame() && FoxMovement.Instance != null)
        {
            if (PauseMenuPanel.activeSelf)
            {
                //disable
                ClosePauseMenu();
            }
            else
            {
                //enable
                CloseAllPanels();
                OpenPauseMenu();
            }
        }
    }
    private void OpenPauseMenu()
    {
        GameEventsManager.instance.PlayerEvents.ToggleInputActions(false);
        PlayerInputHandler.Instance.PauseInput.Enable();
        SaveManager.Instance.SaveGame();
        PauseMenuPanel.SetActive(true);
        FoxMovement.Instance.gameObject.GetComponentInChildren<CinemachineBrain>().m_UpdateMethod = CinemachineBrain.UpdateMethod.FixedUpdate;
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        GeneralMenuPanel.SetActive(true);



        if (QuestMenuManager.TrackedQuest != null && !_activeQuestUI.gameObject.activeSelf)
        {
            _activeQuestUI.gameObject.SetActive(true);
            _activeQuestUI.UpdateQuestMenuUI();
        }
        _sliderValueMaster = PlayerPrefs.GetFloat("MasterVolume");
        _sliderValueMusic = PlayerPrefs.GetFloat("MusicVolume");
        BerryCounter.text = PlayerManager.Instance.Berries + " / " + PlayerManager.Instance.BerryData.Count;
        PineconeCounter.text = PlayerManager.Instance.PineCones + " / " + PlayerManager.Instance.PineConeData.Count;
    }

    void ClosePauseMenu()
    {
        GameEventsManager.instance.PlayerEvents.ToggleInputActions(true);

        Time.timeScale = 1f;
        FoxMovement.Instance.gameObject.GetComponentInChildren<CinemachineBrain>().m_UpdateMethod = CinemachineBrain.UpdateMethod.SmartUpdate;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        PauseMenuPanel?.SetActive(false);
    }
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }



    #region Buttons & Sliders

    public void ChangeSliderMasterVolume(float value)
    {
        _sliderValueMaster = value;
        PlayerPrefs.SetFloat("MasterVolume", _sliderValueMaster);
        _mixer.SetFloat("MasterVolumeMixer", _sliderValueMaster);
        //Debug.Log("value changed");
    }
    public void ChangeSliderMusicVolume(float value)
    {
        _sliderValueMusic = value;
        PlayerPrefs.SetFloat("MusicVolume", _sliderValueMusic);
        _mixer.SetFloat("MusicVolumeMixer", _sliderValueMusic);
        //Debug.Log("value changed");
    }
    public void ChangeSensitivity(float value)
    {
        _sliderValue2 = value;
        PlayerPrefs.SetFloat("sens", _sliderValue2);
        if (_cinemachineFreeLook != null)
        {
            _cinemachineFreeLook.m_XAxis.m_MaxSpeed = PlayerPrefs.GetFloat("sens");
        }
        //Debug.Log("value changed");
    }

    public void CloseAllPanels()
    {
        foreach (var panel in _uiPanels)
        {
            panel.SetActive(false);
        }
    }

    public void Resume()
    {
        Time.timeScale = 1.0f;
        ClosePauseMenu();
    }
    public void ExitQuest()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        if (currentSceneName != "OverWorld - VS")
        {
            //Debug.Log("Quest has been exited. Loading Overworld.");

            GameEventsManager.instance.UIEvents.ShowLoadingScreen(SceneManagerHelper.Scene.Overworld);

            ClosePauseMenu();
        }
    }

    public void ExitGame()
    {
        Application.Quit();
    }
    public void RestartQuest()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        if (currentSceneName != "OverWorld - VS")
        {
            //Debug.Log("Quest has been restarted. Reloading scene.");

            GameEventsManager.instance.UIEvents.ShowLoadingScreen(SceneManagerHelper.GetSceneEnum(currentSceneName));

            ClosePauseMenu();
        }
    }
    public void ChangeYInversion(bool isOn)
    {
        if (_cinemachineFreeLook != null)
        {
            _cinemachineFreeLook.m_YAxis.m_InvertInput = _invertYAxis.isOn;
        }

        if (isOn)
        {
            PlayerPrefs.SetInt("InvertY", 1);
        }
        else
        {
            PlayerPrefs.SetInt("InvertY", 0);
        }
    }
    public void ReturnToMenu()
    {
        Time.timeScale = 1f;

        GameEventsManager.instance.UIEvents.ShowLoadingScreen(SceneManagerHelper.Scene.MainMenu);
    }

    #endregion

    #region Initializers

    void InitializeUIElementsList()
    {
        GameObject[] uiPanelsInit = {
                                    GeneralMenuPanel,
                                    QuestMenuPanel,
                                    OptionsMenuPanel,
                                    MovementControlsMenuPanel,
                                    GamePlayControlsMenuPanel,
                                    SettingsMenuPanel,
                                    ActiveQuestPanel,
                                    RestartQuestPanel,
                                    ExitQuestPanel,
                                    BerryPanel,
                                    PineConePanel
                                    };
        _uiPanels = uiPanelsInit;
        _activeQuestUI = ActiveQuestPanel.GetComponent<ActiveQuestUI>();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        try
        {
            _cinemachineFreeLook = FoxMovement.Instance.gameObject.GetComponentInChildren<CinemachineFreeLook>();
        }
        catch
        {
            _cinemachineFreeLook = null;
        }
        //Debug.Log("Scene loaded: " + scene.name); 
        if ((scene.name == "Overworld" || scene.name == "MainMenu" || scene.name == "Tutorial") && RestartQuestPanel != null && ExitQuestPanel != null)
        {
            //Debug.Log("Scene loaded is Overworld or the main menu. Disabling quest buttons in pause menu.");
            RestartQuestPanel.SetActive(false);
            ExitQuestPanel.SetActive(false);
        }
        else if (RestartQuestPanel != null && ExitQuestPanel != null)
        {
            //Debug.Log("Scene loaded is not overworld. Enabling quest buttons in pause menu.");
            RestartQuestPanel.SetActive(true);
            ExitQuestPanel.SetActive(true);
        }
        if (FoxMovement.Instance != null)
        {

            if (PlayerPrefs.GetFloat("sens") == 0)
            {
                PlayerPrefs.SetFloat("sens", 200);
                _sliderValue2 = PlayerPrefs.GetFloat("sens");
                _sensitivity.value = _sliderValue2;
            }
            else
            {
                _sliderValue2 = PlayerPrefs.GetFloat("sens");
                _sensitivity.value = PlayerPrefs.GetFloat("sens");
                _cinemachineFreeLook.m_XAxis.m_MaxSpeed = PlayerPrefs.GetFloat("sens", _sliderValue2);
            }
        }

    }

    private void DisableQuestButtonsInit()
    {
        if (RestartQuestPanel != null && ExitQuestPanel != null)
        {
            //Debug.Log("Quest buttons have been disabled.");
            RestartQuestPanel.SetActive(false);
            ExitQuestPanel.SetActive(false);
        }
    }

    private void SetDefaultValues()
    {
        try
        {
            _cinemachineFreeLook = FoxMovement.Instance.gameObject.GetComponentInChildren<CinemachineFreeLook>();
        }
        catch
        {
            _cinemachineFreeLook = null;
        }

        if (PlayerPrefs.GetInt("InvertY") == 1 && _cinemachineFreeLook != null)
        {
            _cinemachineFreeLook.m_YAxis.m_InvertInput = true;
        }
        else if (_cinemachineFreeLook != null)
        {
            _cinemachineFreeLook.m_YAxis.m_InvertInput = false;
        }
        _invertYAxis.isOn = PlayerPrefs.GetInt("InvertY") == 1 ? true : false;
        if (PlayerPrefs.GetFloat("sens") == 0)
        {
            PlayerPrefs.SetFloat("sens", 250);
        }
        else
        {
            _sliderValue2 = PlayerPrefs.GetFloat("sens");
            _sensitivity.value = PlayerPrefs.GetFloat("sens");
        }
    }

    #endregion






}
