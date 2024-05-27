using Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenuManager : MonoBehaviour
{
    [SerializeField] public GameObject pauseMenuPanel;
    [SerializeField] GameObject OptionsMenuPanel;
    [SerializeField] GameObject options;
    [SerializeField] GameObject MovementControlsMenuPanel;
    [SerializeField] GameObject GamePlayControlsMenuPanel;
    [SerializeField] GameObject SettingsMenuPanel;
    [SerializeField] CinemachineFreeLook cinemachineFreeLook;
    [SerializeField] Toggle InvertYAxis;
    [SerializeField] Toggle fullscreen;
    [SerializeField] Slider volume, sensitivity;
    private float SliderValue, SliderValue2;

    private bool isInvertErrorLogged = false; // Makes sure the warning that runs in the null check of the InvertYAxis runs once


    void Start()
    {
        pauseMenuPanel = GameObject.Find("PauseMenuEmpty").transform.Find("PauseMenu").gameObject;
        options = GameObject.Find("PauseMenuEmpty").transform.Find("Options").gameObject;
        OptionsMenuPanel = options.transform.Find("OptionsMenu").gameObject;
        MovementControlsMenuPanel = options.transform.Find("MovementControlsMenu").gameObject;
        GamePlayControlsMenuPanel = options.transform.Find("GameplayControlsMenu").gameObject;
        SettingsMenuPanel = options.transform.Find("SettingsMenu").gameObject;
        cinemachineFreeLook = GameObject.Find("FreeLook Camera").GetComponent<CinemachineFreeLook>();
        InvertYAxis = SettingsMenuPanel.transform.Find("InvertCameraTickBox").GetComponent<Toggle>();
        fullscreen = SettingsMenuPanel.transform.Find("FullScreenTickBox").GetComponent<Toggle>();
        volume = SettingsMenuPanel.transform.Find("Volume").GetComponent<Slider>();
        sensitivity = SettingsMenuPanel.transform.Find("Sensitivity").GetComponent<Slider>();
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
            PlayerPrefs.SetFloat("save", 250);
        }
        else
        {
            volume.value = PlayerPrefs.GetFloat("save", SliderValue);
            AudioListener.volume = PlayerPrefs.GetFloat("save", SliderValue);
        }
        if (PlayerPrefs.GetFloat("sens") == 0)
        {
            PlayerPrefs.SetFloat("sens", 250);
        }
        else
        {
            sensitivity.value = PlayerPrefs.GetFloat("sens", SliderValue2);
            cinemachineFreeLook.m_XAxis.m_MaxSpeed = PlayerPrefs.GetFloat("sens", SliderValue2);
        }

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {

            //disable
            if (pauseMenuPanel.activeInHierarchy
                ||OptionsMenuPanel.activeInHierarchy
                ||MovementControlsMenuPanel.activeInHierarchy
                ||GamePlayControlsMenuPanel.activeInHierarchy
                ||SettingsMenuPanel.activeInHierarchy)
            {
                Time.timeScale = 1f;
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                pauseMenuPanel.SetActive(false);
                OptionsMenuPanel.SetActive(false);
                MovementControlsMenuPanel.SetActive(false);
                GamePlayControlsMenuPanel.SetActive(false);
                SettingsMenuPanel.SetActive(false);
            }

            //enable
            else
            {
                SaveManager.instance.SaveGame();
                pauseMenuPanel.SetActive(true);
                Time.timeScale = 0f;
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }
        if (InvertYAxis != null)
        {
            InvertYAxis.onValueChanged.AddListener(delegate { ChangeYInversion(); });
        }
        else
        {
            if (!isInvertErrorLogged)
            {
                Debug.LogError("InvertYAxis is not assigned. Make sure it is assigned in the Inspector. (SettingsMenuPanel is probably not set)");
                isInvertErrorLogged = true;  // Set the flag to true after logging the warning
            }
        }
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
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        pauseMenuPanel.SetActive(false);

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
        OptionsMenuPanel.SetActive(false);
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
    public void ChangeSensitivity(float value)
    {
        SliderValue2 = value;
        PlayerPrefs.SetFloat("sens", SliderValue2);
        cinemachineFreeLook.m_XAxis.m_MaxSpeed = PlayerPrefs.GetFloat("sens");
        Debug.Log("value changed");
    }

}
