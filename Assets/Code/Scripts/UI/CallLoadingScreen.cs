using UnityEngine;
using UnityEngine.Serialization;

public class CallLoadingScreen : MonoBehaviour
{
    [FormerlySerializedAs("newSceneName")] [SerializeField] private SceneManagerHelper.Scene _newSceneName;

    public void ChangeScene()
    {
        GameEventsManager.instance.UIEvents.ShowLoadingScreen(_newSceneName);
    }
}