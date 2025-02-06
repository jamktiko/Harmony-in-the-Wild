using System;

public class UIEvents
{
    public event Action<SceneManagerHelper.Scene> OnShowLoadingScreen;

    public void ShowLoadingScreen(SceneManagerHelper.Scene newSceneName)
    {
        OnShowLoadingScreen?.Invoke(newSceneName);
    }

    public event Action OnHideInstructionPanel;

    public void HideInstructionPanel()
    {
        OnHideInstructionPanel?.Invoke();
    }
}
