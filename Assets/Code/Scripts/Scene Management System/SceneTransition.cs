using UnityEngine;

public class SceneTransition : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] private SceneManagerHelper.Scene goToScene;

    public void GoToScene()
    {
        SceneManagerHelper.LoadScene(goToScene);
    }
}
