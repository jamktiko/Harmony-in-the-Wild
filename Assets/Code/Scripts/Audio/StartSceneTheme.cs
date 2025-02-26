using UnityEngine;
using UnityEngine.Serialization;

public class StartSceneTheme : MonoBehaviour
{
    [FormerlySerializedAs("themeForScene")] [SerializeField] private ThemeName _themeForScene;

    private void Start()
    {
        AudioManager.Instance.StartNewTheme(_themeForScene);
    }
}
