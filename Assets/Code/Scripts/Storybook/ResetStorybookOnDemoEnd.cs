using UnityEngine;

public class ResetStorybookOnDemoEnd : MonoBehaviour
{
    private void Start()
    {
        StorybookHandler.instance.SetNewStorybookData(0, SceneManagerHelper.Scene.Naming, false);
    }
}
