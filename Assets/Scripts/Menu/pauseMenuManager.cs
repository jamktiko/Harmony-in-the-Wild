using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class pauseMenuManager : MonoBehaviour
{
    [SerializeField] GameObject pauseMenuPanel;
    [SerializeField] GameObject OptionsMenuPanel;
    [SerializeField] GameObject options;
    [SerializeField] GameObject MovementControlsMenuPanel;
    [SerializeField] GameObject GamePlayControlsMenuPanel;
    [SerializeField] GameObject SettingsMenuPanel;
    [SerializeField] CinemachineFreeLook cinemachineFreeLook;
    [SerializeField] Toggle InvertYAxis;
    [SerializeField] Toggle fullscreen;
    [SerializeField] Slider volume;
    private float SliderValue;


    void Start()
    {
        pauseMenuPanel = GameObject.Find("Canvas").transform.Find("PauseMenu").gameObject;
        options = GameObject.Find("Canvas").transform.Find("Options").gameObject;
        OptionsMenuPanel = options.transform.Find("OptionsMenu").gameObject;
        MovementControlsMenuPanel = options.transform.Find("MovementControlsMenu").gameObject;
        GamePlayControlsMenuPanel = options.transform.Find("GameplayControlsMenu").gameObject;
        SettingsMenuPanel = options.transform.Find("SettingsMenu").gameObject;
        cinemachineFreeLook = GameObject.Find("FreeLook Camera").GetComponent<CinemachineFreeLook>();
        InvertYAxis = SettingsMenuPanel.transform.Find("InvertCameraTickBox").GetComponent<Toggle>();
        fullscreen = SettingsMenuPanel.transform.Find("FullScreenTickBox").GetComponent<Toggle>();
        volume = SettingsMenuPanel.transform.Find("Volume").GetComponent<Slider>();
        if (PlayerPrefs.GetInt("InvertY") == 1)
        {
            cinemachineFreeLook.m_YAxis.m_InvertInput = true;
        }
        else
        {
            cinemachineFreeLook.m_YAxis.m_InvertInput = false;
        }
        InvertYAxis.isOn = cinemachineFreeLook.m_YAxis.m_InvertInput;
        if (PlayerPrefs.GetFloat("save", SliderValue) == 0)
        {
            PlayerPrefs.SetFloat("save", 100);
        }
        else
        {
            volume.value = PlayerPrefs.GetFloat("save", SliderValue);
            AudioListener.volume = PlayerPrefs.GetFloat("save", SliderValue);
        }

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            //disable
            if (pauseMenuPanel.activeInHierarchy)
            {
                Time.timeScale = 1f;
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                pauseMenuPanel.SetActive(false);
            }
            //enable
            else
            {
                pauseMenuPanel.SetActive(true);
                Time.timeScale = 0f;
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }
        InvertYAxis.onValueChanged.AddListener(delegate { ChangeYInversion(); });
    }
    public void ChangeYInversion()
    {
        cinemachineFreeLook.m_YAxis.m_InvertInput = InvertYAxis.isOn;
        if (InvertYAxis.isOn)
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
        pauseMenuPanel.SetActive(false);
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        
    }
    public void Options()
    {
        OptionsMenuPanel.SetActive(true);
        pauseMenuPanel.SetActive(false);
    }
    public void returnToMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
    public void GameplayMenu()
    {
        GamePlayControlsMenuPanel.SetActive(true);
        pauseMenuPanel.SetActive(false);
        OptionsMenuPanel.SetActive(false);
    }
    public void MovementMenu()
    {
        MovementControlsMenuPanel.SetActive(true);
        pauseMenuPanel.SetActive(false);
        OptionsMenuPanel.SetActive(false);
    }
    public void SettingsMenu()
    {
        SettingsMenuPanel.SetActive(true);
        pauseMenuPanel.SetActive(false);
        OptionsMenuPanel.SetActive(false);
    }
    public void BackButton()
    {
        pauseMenuPanel.SetActive(true);
        SettingsMenuPanel.SetActive(false);
        MovementControlsMenuPanel.SetActive(false);
        GamePlayControlsMenuPanel.SetActive(false);
    }
    public void ChangeSlider(float value)
    {
        SliderValue = value;
        PlayerPrefs.SetFloat("save", SliderValue);
        AudioListener.volume = PlayerPrefs.GetFloat("save");
        Debug.Log("value changed");
    }
}
