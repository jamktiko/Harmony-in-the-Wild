using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenuBookManager : MonoBehaviour
{
    [SerializeField]
    public GameObject pauseMenuPanel,
                        generalMenuPanel,
                        optionsMenuPanel,
                        movementControlsMenuPanel,
                        questMenuPanel,
                        gamePlayControlsMenuPanel,
                        settingsMenuPanel,
                        activeQuestPanel,
                        restartQuestPanel,
                        exitQuestPanel,
                        berryPanel,
                        pineConePanel;

    [SerializeField] CinemachineFreeLook cinemachineFreeLook;

    [SerializeField] Toggle InvertYAxis, 
                            fullscreen;

    [SerializeField] Slider Mastervolume, 
                            MusicVolume, 
                            sensitivity;

    [SerializeField] AudioMixer mixer;

    [SerializeField] private float SliderValueMaster, 
                                   SliderValueMusic, 
                                   SliderValue2;

    [SerializeField] public TMP_Text BerryCounter, 
                                     PineconeCounter;

    [SerializeField] GameObject[] UIPanels;
    [SerializeField] ActiveQuestUI activeQuestUI;

    private void Awake()
    {
        InitializeUIElementsList();
    }

    private void Update()
    {
        if (PlayerInputHandler.instance.PauseInput.WasPressedThisFrame() && FoxMovement.instance != null)
        {
            if (pauseMenuPanel.activeSelf)
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
        GameEventsManager.instance.playerEvents.ToggleInputActions(false);
        PlayerInputHandler.instance.PauseInput.Enable();
        SaveManager.instance.SaveGame();
        pauseMenuPanel.SetActive(true);
        FoxMovement.instance.gameObject.GetComponentInChildren<CinemachineBrain>().m_UpdateMethod = CinemachineBrain.UpdateMethod.FixedUpdate;
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        generalMenuPanel.SetActive(true);

        

        if (QuestMenuManager.trackedQuest != null && !activeQuestUI.gameObject.activeSelf)
        {
            activeQuestUI.gameObject.SetActive(true);
            activeQuestUI.UpdateQuestMenuUI();
        }
        SliderValueMaster = PlayerPrefs.GetFloat("MasterVolume");
        SliderValueMusic = PlayerPrefs.GetFloat("MusicVolume");
        BerryCounter.text = PlayerManager.instance.Berries + " / " + PlayerManager.instance.BerryData.Count;
        PineconeCounter.text = PlayerManager.instance.PineCones + " / " + PlayerManager.instance.PineConeData.Count;
    }

    void ClosePauseMenu()
    {
        GameEventsManager.instance.playerEvents.ToggleInputActions(true);

        Time.timeScale = 1f;
        FoxMovement.instance.gameObject.GetComponentInChildren<CinemachineBrain>().m_UpdateMethod = CinemachineBrain.UpdateMethod.SmartUpdate;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        pauseMenuPanel?.SetActive(false);
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
        SliderValueMaster = value;
        PlayerPrefs.SetFloat("MasterVolume", SliderValueMaster);
        mixer.SetFloat("MasterVolumeMixer", SliderValueMaster);
        //Debug.Log("value changed");
    }
    public void ChangeSliderMusicVolume(float value)
    {
        SliderValueMusic = value;
        PlayerPrefs.SetFloat("MusicVolume", SliderValueMusic);
        mixer.SetFloat("MusicVolumeMixer", SliderValueMusic);
        //Debug.Log("value changed");
    }
    public void ChangeSensitivity(float value)
    {
        SliderValue2 = value;
        PlayerPrefs.SetFloat("sens", SliderValue2);
        if (cinemachineFreeLook != null)
        {
            cinemachineFreeLook.m_XAxis.m_MaxSpeed = PlayerPrefs.GetFloat("sens");
        }
        //Debug.Log("value changed");
    }

    public void CloseAllPanels() 
    {
        foreach (var panel in UIPanels)
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

            GameEventsManager.instance.uiEvents.ShowLoadingScreen(SceneManagerHelper.Scene.Overworld);

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

            GameEventsManager.instance.uiEvents.ShowLoadingScreen(SceneManagerHelper.GetSceneEnum(currentSceneName));

            ClosePauseMenu();
        }
    }
    public void ChangeYInversion(bool isOn)
    {
        if (cinemachineFreeLook != null)
        {
            cinemachineFreeLook.m_YAxis.m_InvertInput = InvertYAxis.isOn;
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
    public void returnToMenu()
    {
        Time.timeScale = 1f;

        GameEventsManager.instance.uiEvents.ShowLoadingScreen(SceneManagerHelper.Scene.MainMenu);
    }

    #endregion

    #region Initializers

    void InitializeUIElementsList()
    {
        GameObject[] UIPanelsInit = { 
                                    generalMenuPanel,
                                    questMenuPanel,
                                    optionsMenuPanel,
                                    movementControlsMenuPanel,
                                    gamePlayControlsMenuPanel,
                                    settingsMenuPanel,
                                    activeQuestPanel,
                                    restartQuestPanel,
                                    exitQuestPanel,
                                    berryPanel,
                                    pineConePanel
                                    };
        UIPanels = UIPanelsInit;
        activeQuestUI = activeQuestPanel.GetComponent<ActiveQuestUI>();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        try
        {
            cinemachineFreeLook = FoxMovement.instance.gameObject.GetComponentInChildren<CinemachineFreeLook>();
        }
        catch
        {
            cinemachineFreeLook = null;
        }
        //Debug.Log("Scene loaded: " + scene.name); 
        if ((scene.name == "Overworld" || scene.name == "MainMenu" || scene.name == "Tutorial") && restartQuestPanel != null && exitQuestPanel != null)
        {
            //Debug.Log("Scene loaded is Overworld or the main menu. Disabling quest buttons in pause menu.");
            restartQuestPanel.SetActive(false);
            exitQuestPanel.SetActive(false);
        }
        else if (restartQuestPanel != null && exitQuestPanel != null)
        {
            //Debug.Log("Scene loaded is not overworld. Enabling quest buttons in pause menu.");
            restartQuestPanel.SetActive(true);
            exitQuestPanel.SetActive(true);
        }
        if (FoxMovement.instance != null)
        {

            if (PlayerPrefs.GetFloat("sens") == 0)
            {
                PlayerPrefs.SetFloat("sens", 200);
                SliderValue2 = PlayerPrefs.GetFloat("sens");
                sensitivity.value = SliderValue2;
            }
            else
            {
                SliderValue2 = PlayerPrefs.GetFloat("sens");
                sensitivity.value = PlayerPrefs.GetFloat("sens");
                cinemachineFreeLook.m_XAxis.m_MaxSpeed = PlayerPrefs.GetFloat("sens", SliderValue2);
            }
        }

    }

    private void DisableQuestButtonsInit()
    {
        if (restartQuestPanel != null && exitQuestPanel != null)
        {
            //Debug.Log("Quest buttons have been disabled.");
            restartQuestPanel.SetActive(false);
            exitQuestPanel.SetActive(false);
        }
    }

    private void SetDefaultValues()
    {
        try
        {
            cinemachineFreeLook = FoxMovement.instance.gameObject.GetComponentInChildren<CinemachineFreeLook>();
        }
        catch
        {
            cinemachineFreeLook = null;
        }

        if (PlayerPrefs.GetInt("InvertY") == 1 && cinemachineFreeLook != null)
        {
            cinemachineFreeLook.m_YAxis.m_InvertInput = true;
        }
        else if (cinemachineFreeLook != null)
        {
            cinemachineFreeLook.m_YAxis.m_InvertInput = false;
        }
        InvertYAxis.isOn = PlayerPrefs.GetInt("InvertY") == 1 ? true : false;
        if (PlayerPrefs.GetFloat("sens") == 0)
        {
            PlayerPrefs.SetFloat("sens", 250);
        }
        else
        {
            SliderValue2 = PlayerPrefs.GetFloat("sens");
            sensitivity.value = PlayerPrefs.GetFloat("sens");
        }
    }

    #endregion






}
