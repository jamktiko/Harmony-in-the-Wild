using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StorybookHandler : MonoBehaviour
{
    private int currentStorybookSectionIndex = 0;
    private string sceneAfterStorybook = "Overworld";

    public void SetNewStorybookData(int index, string nextScene)
    {
        currentStorybookSectionIndex = index;
        sceneAfterStorybook = nextScene;
    }

    public int GetCurrentStorybookSection()
    {
        return currentStorybookSectionIndex;
    }

    public string GetNextScene()
    {
        return sceneAfterStorybook;
    }
}
