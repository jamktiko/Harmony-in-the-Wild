using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DontDestroyOnLoad1 : MonoBehaviour
{
    [SerializeField] GameObject pausemenuManager;
    [SerializeField] GameObject PauseMenuPanel;
    // Start is called before the first frame update
    void Awake()
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag("Manager");
        if (objects.Length > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
            //pausemenuManager = transform.Find("PauseMenuManager").gameObject;
            //PauseMenuPanel = GameObject.Find("PauseMenuEmpty").transform.Find("PauseMenu").gameObject;
        }

    }
    //private void Start()
    //{


    //    if (SceneManager.GetActiveScene().name.Contains("Overworld") || SceneManager.GetActiveScene().name.Contains("Dungeon"))

    //}
    private void OnLevelWasLoaded(int level)
    {
        if (level==0|| level ==1||level==9||level==2||level==11)
        {
            PauseMenuPanel.SetActive(false);
            pausemenuManager.SetActive(false);
        }
        else
        {
            pausemenuManager.SetActive(true);
        }
    }
}
