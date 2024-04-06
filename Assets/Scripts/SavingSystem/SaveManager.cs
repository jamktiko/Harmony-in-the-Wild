using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

//This script handles both saving and loading of gameData.

public class SaveManager : MonoBehaviour
{
    public static SaveManager instance;

    private string saveFilePath;

    private GameData gameData = new GameData();
    public string testInternal;

    private void Awake()
    {
        saveFilePath = Application.persistentDataPath + "/GameData.json";

        if(instance != null)
        {
            Debug.LogWarning("There is more than one Save Manager.");
            Destroy(gameObject);
        }

        instance = this;

        LoadGame();
    }

    private void Update()
    {
#if DEBUG
        if (Input.GetKeyDown(KeyCode.O))
        {
            SaveGame();
        }

        if (Input.GetKey(KeyCode.X) && Input.GetKeyDown(KeyCode.Z))
        {
            DeleteSave();
        }
#endif
    }

    public void SaveGame()
    {
        CollectDataForSaving();
        GameData dataToSave = new GameData();

        dataToSave.questData = gameData.questData;
        dataToSave.abilityData = gameData.abilityData;

        string jsonData = JsonUtility.ToJson(dataToSave);
        File.WriteAllText(saveFilePath, jsonData);

        Debug.Log("Game saved.");
    }
    private void LoadGame()
    {
        if (File.Exists(saveFilePath))
        {
            string jsonData = File.ReadAllText(saveFilePath);
            GameData loadedData = JsonUtility.FromJson<GameData>(jsonData);

            gameData.questData = loadedData.questData;
            gameData.abilityData = loadedData.abilityData;
            //gameData.playerPositionData = loadedData.playerPositionData;

            Debug.Log("Game loaded.");
        }
    }
    #region CollectDataForSaving
    private void CollectDataForSaving()
    {
        CollectQuestData();
        CollectAbilityData();
        //CollectPlayerPositionData();
    }

    private void CollectQuestData()
    {
        gameData.questData = QuestManager.instance.CollectQuestDataForSaving();
    }

    private void CollectAbilityData()
    {
        gameData.abilityData = AbilityManager.instance.CollectAbilityDataForSaving();

    }
    //private void CollectPlayerPositionData()
    //{
    //    string activeSceneName = SceneManager.GetActiveScene().name;
    //    string overworldSceneName = SceneManagerHelper.GetSceneName(SceneManagerHelper.Scene.Overworld);

    //    if (activeSceneName == overworldSceneName)
    //    {
    //        gameData.playerPositionData = FoxMovement.instance.CollectPlayerPositionForSaving();
    //    }
    //    else
    //    {
    //        gameData.playerPositionData = new List<float> { 1627f, 118f, 360f };
    //    }
    //}
    #endregion

    #region GetDataForLoading
    public List<string> GetLoadedData(string dataType)
    {
        List<string> data = new List<string>();

        if (File.Exists(saveFilePath))
        {
            switch (dataType)
            {
                case "quest":
                    data = gameData.questData;
                    break;

                default:
                    Debug.LogWarning("No type match found when trying to load saved data for " + dataType);
                    break;
            }
        }

        return data;
    }
    public Dictionary<Abilities, bool> GetLoadedAbilityData()
    {
        Dictionary<Abilities, bool> data = new Dictionary<Abilities, bool>();

        // fetch the saved data from the file if there is a previous save
        if (File.Exists(saveFilePath))
        {
            data = gameData.abilityData;
        }

        // if there isn't, return all abilities false
        else
        {
            foreach (Abilities ability in Enum.GetValues(typeof(Abilities)))
            {
                data.Add(ability, false);
            }
        }
        return data;
    }

    //public List<float> GetLoadedPlayerPositionData()
    //{
    //    List<float> data = new List<float>();

    //    // fetch the saved data from the file if there is a previous save
    //    if (File.Exists(saveFilePath))
    //    {
    //        data = gameData.playerPositionData;
    //    }

    //    // if there isn't, return default position
    //    else
    //    {
    //        data=new List<float> { 1627f,118f,360f };
    //    }

    //    return data;
    //}
    #endregion

    private void DeleteSave()
    {
        File.Delete(saveFilePath);

        Debug.LogError("The save file has been deleted. Please restart the game to avoid any errors.");
    }
}