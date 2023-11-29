using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StorybookHandler : MonoBehaviour
{
    private int currentStorybookSectionIndex = 1;
    private string sceneAfterStorybook = "Overworld";
    private bool isDungeonEndStory = true;

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
