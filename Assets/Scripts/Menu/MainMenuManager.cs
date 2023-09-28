using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] GameObject options;
    private void Start()
    {
        options = GameObject.Find("OptionsMenu");
    }
    public void StartNewGame() 
    {
        SceneManager.LoadScene(1); 
    }
    public void LoadGame() 
    {
        //load
    }
    public void ExitGame() 
    {
        Application.Quit();
    }
    public void Options() 
    {
        options.SetActive(true);
    }
    public void Credits() 
    {
        
    }
    public void BackButton() 
    {
        options.SetActive(false);
    }
}
