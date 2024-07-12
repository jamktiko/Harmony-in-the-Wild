using UnityEngine.SceneManagement;
using System.Collections.Generic;

public static class SceneManagerHelper
{
    //Define an enum to represent all the scenes
    public enum Scene
    {
        Disclaimer,
        MainMenu,
        Storybook,
        Overworld,
        Dungeon_Frog_Learning,
        Dungeon_Frog_Boss,
        Dungeon_Squirrel_Learning,
        Dungeon_Squirrel_Boss,
        Dungeon_Penguin,
        Naming,
        Dungeon_Monke_Learning,
        Credits,
        Tutorial
    }

    //Map enum values to scene names
    private static readonly Dictionary<Scene, string> sceneNames = new Dictionary<Scene, string>
    {
        {Scene.Disclaimer, "Disclaimer" },
        {Scene.MainMenu, "MainMenu" },
        {Scene.Storybook, "Storybook" },
        {Scene.Overworld, "Overworld" },
        {Scene.Dungeon_Frog_Learning, "Dungeon_Frog_Learning" },
        {Scene.Dungeon_Frog_Boss, "Dungeon_Frog_Boss" },
        {Scene.Dungeon_Squirrel_Learning, "Dungeon_Squirrel_Learning" },
        {Scene.Dungeon_Squirrel_Boss, "Dungeon_Squirrel_Boss" },
        {Scene.Dungeon_Penguin, "Dungeon_Penguin" },
        {Scene.Naming, "Naming" },
        {Scene.Dungeon_Monke_Learning, "Dungeon_Monke_Learning" },
        {Scene.Credits, "Credits" },
        {Scene.Tutorial, "Tutorial" }
    };

    //Method to get the scene name from an enum
    public static string GetSceneName(Scene scene)
    {
        return sceneNames[scene];
    }

    //Method to load a scene by enum
    public static void LoadScene(Scene scene)
    {
        SceneManager.LoadScene(sceneNames[scene]);
    }

    //Method to load a scene asynchronously if needed
    public static void LoadSceneAsync(Scene scene)
    {
        SceneManager.LoadSceneAsync(sceneNames[scene]);
    }
}

