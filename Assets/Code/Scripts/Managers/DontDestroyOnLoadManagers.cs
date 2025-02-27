using UnityEngine;
using UnityEngine.Serialization;

public class DontDestroyOnLoadManagers : MonoBehaviour
{
    [FormerlySerializedAs("pauseMenuManager")] [SerializeField]
    private GameObject _pauseMenuManager;
    [FormerlySerializedAs("PauseMenuPanel")] [SerializeField]
    private GameObject _pauseMenuPanel;
    public static DontDestroyOnLoadManagers Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
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
