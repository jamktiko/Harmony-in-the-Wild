using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyOnLoadMenu : MonoBehaviour
{
    public static DontDestroyOnLoadMenu instance;
     [SerializeField]private GameObject pauseMenuPanel;

    private void Awake()
    {
        if (instance==null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void OnLevelWasLoaded(int level)
    {
        if (level == 0)
        {
            pauseMenuPanel.SetActive(false);
        }
    }
}
