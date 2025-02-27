using Cinemachine;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class PauseMenuManager : MonoBehaviour
{
    [FormerlySerializedAs("pauseMenuPanel")] [SerializeField] public GameObject PauseMenuPanel;
    [FormerlySerializedAs("OptionsMenuPanel")] [SerializeField]
    private GameObject _optionsMenuPanel;
    [FormerlySerializedAs("options")] [SerializeField]
    private GameObject _options;
    [FormerlySerializedAs("MovementControlsMenuPanel")] [SerializeField]
    private GameObject _movementControlsMenuPanel;
    [FormerlySerializedAs("GamePlayControlsMenuPanel")] [SerializeField]
    private GameObject _gamePlayControlsMenuPanel;
    [FormerlySerializedAs("SettingsMenuPanel")] [SerializeField]
    private GameObject _settingsMenuPanel;
    [FormerlySerializedAs("restartQuestPanel")] [SerializeField]
    private GameObject _restartQuestPanel;
    [FormerlySerializedAs("exitQuestPanel")] [SerializeField]
    private GameObject _exitQuestPanel;
    [FormerlySerializedAs("cinemachineFreeLook")] [SerializeField]
    private CinemachineFreeLook _cinemachineFreeLook;
    [FormerlySerializedAs("InvertYAxis")] [SerializeField]
    private Toggle _invertYAxis;
    [FormerlySerializedAs("fullscreen")] [SerializeField]
    private Toggle _fullscreen;
    [FormerlySerializedAs("Mastervolume")] [SerializeField]
    private Slider _mastervolume;
    [FormerlySerializedAs("MusicVolume")] [SerializeField]
    private Slider _musicVolume;
    [FormerlySerializedAs("sensitivity")] [SerializeField]
    private Slider _sensitivity;
    [FormerlySerializedAs("mixer")] [SerializeField]
    private AudioMixer _mixer;
    [FormerlySerializedAs("SliderValueMaster")] [SerializeField] private float _sliderValueMaster;
    [FormerlySerializedAs("SliderValueMusic")] [SerializeField] private float _sliderValueMusic;
    [FormerlySerializedAs("SliderValue2")] [SerializeField] private float _sliderValue2;
    [SerializeField] public TMP_Text BerryCounter, PineconeCounter;


    private bool _isInvertErrorLogged = false; // Makes sure the warning that runs in the null check of the InvertYAxis runs once


    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void Start()
    {

        PopulateFields();

        DisableQuestButtonsInit();

    }

    private void Update()
    {
        if (PlayerInputHandler.Instance.PauseInput.WasPressedThisFrame() && FoxMovement.Instance != null)
        {

            //disable
            if (PauseMenuPanel.activeInHierarchy
                || _optionsMenuPanel.activeInHierarchy
                || _movementControlsMenuPanel.activeInHierarchy
                || _gamePlayControlsMenuPanel.activeInHierarchy
                || _settingsMenuPanel.activeInHierarchy)
            {
                Debug.Log("Disable pause menu.");
                DisablePauseMenu();
            }

            //enable
            else
            {
                EnablePauseMenu();
            }
        }
        //if (InvertYAxis != null)
        //{
        //    InvertYAxis.onValueChanged.AddListener(delegate { ChangeYInversion(); });
        //}
        //else
        //{
        //    if (!isInvertErrorLogged)
        //    {
        //        Debug.LogError("InvertYAxis is not assigned. Make sure it is assigned in the Inspector. (SettingsMenuPanel is probably not set)");
        //        isInvertErrorLogged = true;  // Set the flag to true after logging the warning
        //    }
        //}
    }

    private void EnablePauseMenu()
    {
        GameEventsManager.instance.PlayerEvents.ToggleInputActions(false);
        PlayerInputHandler.Instance.PauseInput.Enable();
        SaveManager.Instance.SaveGame();
        PauseMenuPanel.SetActive(true);
        FoxMovement.Instance.gameObject.GetComponentInChildren<CinemachineBrain>().m_UpdateMethod = CinemachineBrain.UpdateMethod.FixedUpdate;
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        _sliderValueMaster = PlayerPrefs.GetFloat("MasterVolume");
        _sliderValueMusic = PlayerPrefs.GetFloat("MusicVolume");
        BerryCounter.text = PlayerManager.Instance.Berries + " / " + PlayerManager.Instance.BerryData.Count;
        PineconeCounter.text = PlayerManager.Instance.PineCones + " / " + PlayerManager.Instance.PineConeData.Count;
    }

    private void DisablePauseMenu()
    {
        GameEventsManager.instance.PlayerEvents.ToggleInputActions(true);

        Time.timeScale = 1f;
        FoxMovement.Instance.gameObject.GetComponentInChildren<CinemachineBrain>().m_UpdateMethod = CinemachineBrain.UpdateMethod.SmartUpdate;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        PauseMenuPanel.SetActive(false);
        _optionsMenuPanel.SetActive(false);
        _movementControlsMenuPanel.SetActive(false);
        _gamePlayControlsMenuPanel.SetActive(false);
        _settingsMenuPanel.SetActive(false);
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
    public void ExitGame()
    {
        Application.Quit();
    }
    public void Resume()
    {
        GameEventsManager.instance.PlayerEvents.ToggleInputActions(true);

        Time.timeScale = 1f;
        FoxMovement.Instance.gameObject.GetComponentInChildren<CinemachineBrain>().m_UpdateMethod = CinemachineBrain.UpdateMethod.SmartUpdate;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        PauseMenuPanel.SetActive(false);

    }
    public void Options()
    {
        _optionsMenuPanel.SetActive(true);
        PauseMenuPanel.SetActive(false);
    }
    public void ReturnToMenu()
    {
        Time.timeScale = 1f;

        GameEventsManager.instance.UIEvents.ShowLoadingScreen(SceneManagerHelper.Scene.MainMenu);
    }
    public void GameplayMenu()
    {
        _gamePlayControlsMenuPanel.SetActive(true);
        PauseMenuPanel.SetActive(false);
        _optionsMenuPanel.SetActive(false);
    }
    public void MovementMenu()
    {
        _movementControlsMenuPanel.SetActive(true);
        PauseMenuPanel.SetActive(false);
        _optionsMenuPanel.SetActive(false);
    }
    public void SettingsMenu()
    {
        _settingsMenuPanel.SetActive(true);
        _mastervolume.value = _sliderValueMaster;
        _musicVolume.value = _sliderValueMusic;
        PauseMenuPanel.SetActive(false);
        _optionsMenuPanel.SetActive(false);
    }
    public void BackButton()
    {
        PauseMenuPanel.SetActive(true);
        _optionsMenuPanel.SetActive(false);
        _settingsMenuPanel.SetActive(false);
        _movementControlsMenuPanel.SetActive(false);
        _gamePlayControlsMenuPanel.SetActive(false);
    }
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

    public void ExitQuest()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        if (currentSceneName != "OverWorld - VS")
        {
            //Debug.Log("Quest has been exited. Loading Overworld.");

            GameEventsManager.instance.UIEvents.ShowLoadingScreen(SceneManagerHelper.Scene.Overworld);

            Resume();
        }
    }

    public void RestartQuest()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        if (currentSceneName != "OverWorld - VS")
        {
            //Debug.Log("Quest has been restarted. Reloading scene.");

            GameEventsManager.instance.UIEvents.ShowLoadingScreen(SceneManagerHelper.GetSceneEnum(currentSceneName));

            Resume();
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        PopulateFields();
        try
        {
            _cinemachineFreeLook = FoxMovement.Instance.gameObject.GetComponentInChildren<CinemachineFreeLook>();
        }
        catch
        {
            _cinemachineFreeLook = null;
        }
        //Debug.Log("Scene loaded: " + scene.name); 
        if ((scene.name == "Overworld" || scene.name == "MainMenu" || scene.name == "Tutorial") && _restartQuestPanel != null && _exitQuestPanel != null)
        {
            //Debug.Log("Scene loaded is Overworld or the main menu. Disabling quest buttons in pause menu.");
            _restartQuestPanel.SetActive(false);
            _exitQuestPanel.SetActive(false);
        }
        else if (_restartQuestPanel != null && _exitQuestPanel != null)
        {
            //Debug.Log("Scene loaded is not overworld. Enabling quest buttons in pause menu.");
            _restartQuestPanel.SetActive(true);
            _exitQuestPanel.SetActive(true);
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
        if (_restartQuestPanel != null && _exitQuestPanel != null)
        {
            //Debug.Log("Quest buttons have been disabled.");
            _restartQuestPanel.SetActive(false);
            _exitQuestPanel.SetActive(false);
        }
    }

    private void PopulateFields()
    {
        PauseMenuPanel = GameObject.Find("PauseMenuEmpty").transform.Find("PauseMenu").gameObject;
        _options = GameObject.Find("PauseMenuEmpty").transform.Find("Options").gameObject;
        _optionsMenuPanel = _options.transform.Find("OptionsMenu").gameObject;
        _movementControlsMenuPanel = _options.transform.Find("MovementControlsMenu").gameObject;
        _gamePlayControlsMenuPanel = _options.transform.Find("GameplayControlsMenu").gameObject;
        _settingsMenuPanel = _options.transform.Find("SettingsMenu").gameObject;
        _restartQuestPanel = PauseMenuPanel.transform.Find("RestartQuestButton").gameObject;
        _exitQuestPanel = PauseMenuPanel.transform.Find("ExitQuestButton").gameObject;
        _invertYAxis = _settingsMenuPanel.transform.Find("InvertCameraTickBox").GetComponent<Toggle>();
        _fullscreen = _settingsMenuPanel.transform.Find("FullScreenTickBox").GetComponent<Toggle>();
        _mastervolume = _settingsMenuPanel.transform.Find("MasterVolume").GetComponent<Slider>();
        _musicVolume = _settingsMenuPanel.transform.Find("MusicVolume").GetComponent<Slider>();
        _sensitivity = _settingsMenuPanel.transform.Find("Sensitivity").GetComponent<Slider>();
        _sliderValueMaster = PlayerPrefs.GetFloat("MasterVolume");
        _sliderValueMusic = PlayerPrefs.GetFloat("MusicVolume");
        _mastervolume.value = _sliderValueMaster;
        _musicVolume.value = _sliderValueMusic;
        BerryCounter = PauseMenuPanel.transform.Find("BerryCounter").GetChild(1).GetComponent<TMP_Text>();
        PineconeCounter = PauseMenuPanel.transform.Find("PineconeCounter").GetChild(1).GetComponent<TMP_Text>();
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
}
