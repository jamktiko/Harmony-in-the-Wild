using UnityEngine;
using UnityEngine.Serialization;

public class StorybookHandler : MonoBehaviour
{
    public static StorybookHandler Instance;

    private int _currentStorybookSectionIndex = 0;
    private bool _isDungeonEndStory = false;

    [FormerlySerializedAs("sceneAfterStorybook")] public SceneManagerHelper.Scene SceneAfterStorybook = SceneManagerHelper.Scene.Naming;

    private void Awake()
    {
        // creating the instance for Storybook Handler
        if (Instance != null)
        {
            Debug.LogWarning("There is more than one Game Events Manager in the scene");
            Destroy(gameObject);
        }

        else
        {
            Instance = this;

        }
    }

    public void SetNewStorybookData(int index, SceneManagerHelper.Scene nextScene, bool isDungeonEnding)
    {
        _currentStorybookSectionIndex = index;
        SceneAfterStorybook = nextScene;
        _isDungeonEndStory = isDungeonEnding;
    }

    public int GetCurrentStorybookSection()
    {
        return _currentStorybookSectionIndex;
    }

    public SceneManagerHelper.Scene GetNextScene()
    {
        return SceneAfterStorybook;
    }

    public bool CheckForDungeonEnding()
    {
        return _isDungeonEndStory;
    }
}
