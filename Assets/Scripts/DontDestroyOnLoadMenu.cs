using UnityEngine;

public class DontDestroyOnLoadMenu : MonoBehaviour
{
    public static DontDestroyOnLoadMenu instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

        }
        else
        {
            Destroy(gameObject);
        }
    }

}
