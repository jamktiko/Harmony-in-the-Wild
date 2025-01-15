using UnityEngine;

public class StorybookHandler : MonoBehaviour
{
    public static StorybookHandler instance;

    private int currentStorybookSectionIndex = 0;
    private bool isDungeonEndStory = false;

    public SceneManagerHelper.Scene sceneAfterStorybook = SceneManagerHelper.Scene.Naming;

    private void Awake()
    {
        // creating the instance for Storybook Handler
        if (instance != null)
        {
            Debug.LogWarning("There is more than one Game Events Manager in the scene");
            Destroy(gameObject);
        }

        else
        {
            instance = this;

        }
    }

    public void SetNewStorybookData(int index, SceneManagerHelper.Scene nextScene, bool isDungeonEnding)
    {
        currentStorybookSectionIndex = index;
        sceneAfterStorybook = nextScene;
        isDungeonEndStory = isDungeonEnding;
    }

    public int GetCurrentStorybookSection()
    {
        return currentStorybookSectionIndex;
    }

    public SceneManagerHelper.Scene GetNextScene()
    {
        return sceneAfterStorybook;
    }

    public bool CheckForDungeonEnding()
    {
        return isDungeonEndStory;
    }
}
