using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StorybookHandler : MonoBehaviour
{
    private int currentStorybookSectionIndex = 0;
    private string sceneAfterStorybook = "Overworld";
    private bool isDungeonEndStory = false;

    public static StorybookHandler instance;

    private void Awake()
    {
        // creating the instance for Storybook Handler
        if (instance != null)
        {
            Debug.LogWarning("There is more than one Storybook Handler in the scene!");
            Destroy(gameObject);
        }

        instance = this;
    }

    public void SetNewStorybookData(int index, string nextScene, bool isDungeonEnding)
    {
        currentStorybookSectionIndex = index;
        sceneAfterStorybook = nextScene;
        isDungeonEndStory = isDungeonEnding;
    }

    public int GetCurrentStorybookSection()
    {
        return currentStorybookSectionIndex;
    }

    public string GetNextScene()
    {
        return sceneAfterStorybook;
    }

    public bool CheckForDungeonEnding()
    {
        return isDungeonEndStory;
    }
}
