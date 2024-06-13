using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using Newtonsoft.Json;

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
        dataToSave.playerPositionData = gameData.playerPositionData;
        dataToSave.treeOfLifeState = gameData.treeOfLifeState;
        dataToSave.dialogueVariableData = gameData.dialogueVariableData;

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
            gameData.playerPositionData = loadedData.playerPositionData;
            gameData.treeOfLifeState = loadedData.treeOfLifeState;
            gameData.dialogueVariableData = loadedData.dialogueVariableData;

            Debug.Log("Game loaded.");
        }
    }
    #region CollectDataForSaving
    private void CollectDataForSaving()
    {
        CollectQuestData();
        CollectAbilityData();
        CollectPlayerPositionData();
        CollectTreeOfLifeState();
        CollectDialogueVariableData();
    }

    private void CollectQuestData()
    {
        gameData.questData = QuestManager.instance.CollectQuestDataForSaving();
    }

    private void CollectAbilityData()
    {
        gameData.abilityData = AbilityManager.instance.CollectAbilityDataForSaving();
    }

    private void CollectTreeOfLifeState()
    {
        if(SceneManager.GetActiveScene().name == SceneManagerHelper.GetSceneName(SceneManagerHelper.Scene.Overworld))
        {
            gameData.treeOfLifeState = TreeOfLifeState.instance.GetTreeOfLifeState();
        }
    }

    private void CollectDialogueVariableData()
    {
        gameData.dialogueVariableData = DialogueManager.instance.CollectDialogueVariableDataForSaving();
    }

    public int GetTreeOfLifeState()
    {
        return gameData.treeOfLifeState;
    }

    public void CollectPlayerPositionData()
    {
        gameData.playerPositionData = FoxMovement.instance.CollectPlayerPositionForSaving();
        Debug.Log("saving position");
    }
    #endregion

    #region GetDataForLoading
    public List<string> GetLoadedQuestData(string dataType)
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
    public Dictionary<Abilities, bool> GetLoadedAbilityDictionary()
    {
        // Create a new Dictionary<Abilities, bool> to store the loaded data
        Dictionary<Abilities, bool> loadedDictionary = new Dictionary<Abilities, bool>();

        if (File.Exists(saveFilePath))
        {
            // Read the JSON string from the file
            string json = File.ReadAllText(saveFilePath);

            // Deserialize the JSON string into a Dictionary<string, bool>

            GameData gameData = JsonConvert.DeserializeObject<GameData>(json);

            loadedDictionary = JsonConvert.DeserializeObject<Dictionary<Abilities, bool>>(gameData.abilityData);
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

    public string GetLoadedDialogueVariables()
    {
        return gameData.dialogueVariableData;
    }

    public List<float> GetLoadedPlayerPosition()
    {
        List<float> data = new List<float>();

        //fetch the saved data from the file if there is a previous save, else use default starting position
        if (File.Exists(saveFilePath))
        {
            Debug.Log("loadplayerpos data: " + data + data.Count);

            string json = File.ReadAllText(saveFilePath);
            GameData gameData = JsonConvert.DeserializeObject<GameData>(json);

            data = JsonConvert.DeserializeObject<List<float>>(gameData.playerPositionData);
        }
        else
        {
            data = new List<float> { 1627f, 118f, 360f };
        }

        return data;
    }
    #endregion

    public void DeleteSave()
    {
        File.Delete(saveFilePath);

        Debug.Log("Save file deleted.");
    }
}