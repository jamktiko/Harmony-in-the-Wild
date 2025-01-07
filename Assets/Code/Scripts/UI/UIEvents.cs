using System;

public class UIEvents
{
    public event Action<string> OnShowLoadingScreen;

    public void ShowLoadingScreen(string newSceneName)
    {
        OnShowLoadingScreen?.Invoke(newSceneName);
    }
}
