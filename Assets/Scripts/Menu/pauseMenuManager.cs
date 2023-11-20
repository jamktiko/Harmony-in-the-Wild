using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class pauseMenuManager : MonoBehaviour
{
    [SerializeField] GameObject pauseMenuPanel;
    [SerializeField] CinemachineFreeLook cinemachineFreeLook;
    [SerializeField] Toggle InvertYAxis;
    void Start()
    {
        pauseMenuPanel = GameObject.Find("Canvas").transform.Find("PauseMenu").gameObject;
        cinemachineFreeLook = GameObject.Find("FreeLook Camera").GetComponent<CinemachineFreeLook>();
        InvertYAxis = pauseMenuPanel.GetComponentInChildren<Toggle>();
        if (PlayerPrefs.GetInt("InvertY") == 1)
        {
            cinemachineFreeLook.m_YAxis.m_InvertInput = true;
        }
        else
        {
            cinemachineFreeLook.m_YAxis.m_InvertInput = false;
        }
        InvertYAxis.isOn = cinemachineFreeLook.m_YAxis.m_InvertInput;


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
    public void Settings()
    {
        //update later
    }
    public void returnToMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}
