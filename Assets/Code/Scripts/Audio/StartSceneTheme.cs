using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartSceneTheme : MonoBehaviour
{
    [SerializeField] private ThemeName themeForScene;

    private void Start()
    {
        AudioManager.instance.StartNewTheme(themeForScene);
    }
}
