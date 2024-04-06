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
    // Load dictionary from JSON
    public Dictionary<Abilities, bool> LoadDictionaryFromJson()
    {
        // Create a new Dictionary<Abilities, bool> to store the loaded data
        Dictionary<Abilities, bool> loadedDictionary = new Dictionary<Abilities, bool>();

        if (File.Exists(saveFilePath))
        {
            // Read the JSON string from the file
            string json = File.ReadAllText(saveFilePath);

            // Deserialize the JSON string into a Dictionary<string, bool>
            Dictionary<string, bool> stringDictionary = JsonUtility.FromJson<Dictionary<string, bool>>(json);


            // Convert string keys back to Abilities enum and populate the loaded dictionary
            foreach (var kvp in stringDictionary)
            {
                if (Enum.TryParse(kvp.Key, out Abilities ability))
                {
                    loadedDictionary[ability] = kvp.Value;
                }
                else
                {
                    Debug.LogWarning($"Failed to parse key '{kvp.Key}' to Abilities enum");
                }
            }
        }
        else
        {
            //if no savefile, populate the dictionary with disabled abilities
            foreach (Abilities ability in Enum.GetValues(typeof(Abilities)))
            {
                loadedDictionary.Add(ability, false);
            }
        }
            return loadedDictionary;
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