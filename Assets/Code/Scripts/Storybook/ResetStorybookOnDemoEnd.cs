using UnityEngine;

public class ResetStorybookOnDemoEnd : MonoBehaviour
{
    private void Start()
    {
        StorybookHandler.Instance.SetNewStorybookData(0, SceneManagerHelper.Scene.Naming, false);
    }
}
