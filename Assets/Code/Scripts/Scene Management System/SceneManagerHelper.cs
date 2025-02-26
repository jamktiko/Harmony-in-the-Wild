using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEngine;

public static class SceneManagerHelper
{
    //Define an enum to represent all the scenes
    public enum Scene
    {
        NoName,
        Disclaimer,
        MainMenu,
        Storybook,
        Overworld,
        Overworld_VS,
        Dungeon_Frog_Learning,
        Dungeon_Frog_Boss,
        Dungeon_Squirrel_Learning,
        Dungeon_Squirrel_Boss,
        Dungeon_Penguin,
        DemoEnd,
        Naming,
        Dungeon_Monkey_Learning,
        Credits,
        Tutorial
    }

    //Map enum values to scene names
    private static readonly Dictionary<Scene, string> sceneNames = new Dictionary<Scene, string>
    {
        {Scene.NoName, "" },
        {Scene.Disclaimer, "Disclaimer" },
        {Scene.MainMenu, "MainMenu" },
        {Scene.Storybook, "Storybook" },
        {Scene.Overworld, "Overworld" },
        {Scene.Overworld_VS, "OverWorld - VS"},
        {Scene.Dungeon_Frog_Learning, "Dungeon_Frog_Learning" },
        {Scene.Dungeon_Frog_Boss, "Dungeon_Frog_Boss" },
        {Scene.Dungeon_Squirrel_Learning, "Dungeon_Squirrel_Learning" },
        {Scene.Dungeon_Squirrel_Boss, "Dungeon_Squirrel_Boss" },
        {Scene.Dungeon_Penguin, "Dungeon_Penguin" },
        {Scene.DemoEnd, "DemoEnd" },
        {Scene.Naming, "Naming" },
        {Scene.Dungeon_Monkey_Learning, "Dungeon_Monke_Learning_NEW" },
        {Scene.Credits, "Credits" },
        {Scene.Tutorial, "Tutorial" }
    };

    //Method to get the scene name from an enum
    public static string GetSceneName(Scene scene)
    {
        return sceneNames[scene];
    }
    
    public static Scene GetSceneEnum(string sceneName)
    {
        Scene currentScene = new Scene();

        foreach(var keyValuePair in sceneNames)
        {
            if(keyValuePair.Value == sceneName)
            {
                currentScene = keyValuePair.Key;
            }
        }

        return currentScene;
    }

    //Method to load a scene by enum
    public static void LoadScene(Scene scene)
    {
        SceneManager.LoadScene(sceneNames[scene]);
    }

    //Method to load a scene asynchronously if needed
    public static AsyncOperation LoadSceneAsync(Scene scene)
    {
        return SceneManager.LoadSceneAsync(sceneNames[scene]);
    }
}

