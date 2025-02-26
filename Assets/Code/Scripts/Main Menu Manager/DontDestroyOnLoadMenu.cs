using UnityEngine;

public class DontDestroyOnLoadMenu : MonoBehaviour
{
    public static DontDestroyOnLoadMenu Instance;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

        }
        else
        {
            Destroy(gameObject);
        }
    }

}
