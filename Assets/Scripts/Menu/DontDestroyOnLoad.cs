using UnityEngine;

public class DontDestroyOnLoad : MonoBehaviour
{
    [SerializeField] GameObject pauseMenuManager;
    [SerializeField] GameObject PauseMenuPanel;

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
        }
    }

    private void OnLevelWasLoaded(int level)
    {
        if (level==0|| level ==1||level==9||level==2||level==11)
        {
            PauseMenuPanel.SetActive(false);
            pauseMenuManager.SetActive(false);
        }
        else
        {
            pauseMenuManager.SetActive(true);
        }
    }
}
