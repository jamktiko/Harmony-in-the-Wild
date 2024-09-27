using UnityEngine;

public class DontDestroyOnLoadManagers : MonoBehaviour
{
    [SerializeField] GameObject pauseMenuManager;
    [SerializeField] GameObject PauseMenuPanel;
    public static DontDestroyOnLoadManagers instance;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            Debug.LogWarning("Managers initialized");
        }
        else
        {
            Debug.LogWarning("Managers already initialized, destroying extra");
            Destroy(gameObject);
        }
    }

    //private void OnLevelWasLoaded(int level)
    //{
    //    if (level==0|| level ==1||level==9||level==2||level==11)
    //    {
    //        PauseMenuPanel.SetActive(false);
    //        pauseMenuManager.SetActive(false);
    //    }
    //    else
    //    {
    //        pauseMenuManager.SetActive(true);
    //    }
    //}
}
