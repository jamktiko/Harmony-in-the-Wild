using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CallLoadingScreen : MonoBehaviour
{
    [SerializeField] private string newSceneName;

    public void ChangeScene()
    {
        GameEventsManager.instance.uiEvents.ShowLoadingScreen(newSceneName);
    }
}
