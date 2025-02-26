using UnityEngine;

public class StartSceneTheme : MonoBehaviour
{
    [SerializeField] private ThemeName themeForScene;

    private void Start()
    {
        AudioManager.Instance.StartNewTheme(themeForScene);
    }
}
