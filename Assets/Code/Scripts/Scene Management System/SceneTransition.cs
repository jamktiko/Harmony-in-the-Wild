using UnityEngine;
using UnityEngine.Serialization;

public class SceneTransition : MonoBehaviour
{
    [FormerlySerializedAs("goToScene")]
    [Header("Config")]
    [SerializeField] private SceneManagerHelper.Scene _goToScene;

    public void GoToScene()
    {
        SceneManagerHelper.LoadScene(_goToScene);
    }
}
