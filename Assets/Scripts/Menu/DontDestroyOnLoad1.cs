using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DontDestroyOnLoad1 : MonoBehaviour
{
    [SerializeField] GameObject pausemenuManager;
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
            pausemenuManager = transform.Find("PauseMenuManager").gameObject;
        }

    }
    //private void Start()
    //{


    //    if (SceneManager.GetActiveScene().name.Contains("Overworld") || SceneManager.GetActiveScene().name.Contains("Dungeon"))

    //}
    private void OnLevelWasLoaded(int level)
    {
        if (level==0|| level ==1||level==8)
        {
            pausemenuManager.SetActive(false);
        }
        else
        {
            pausemenuManager.SetActive(true);
        }
    }
}
