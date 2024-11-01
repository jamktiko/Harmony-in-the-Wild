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

    public string saveFilePath;

    public GameData gameData = new GameData();

    public static bool isSaving;

    private void Awake()
    {
        saveFilePath = Application.persistentDataPath + "/GameData.json";

        if (SaveManager.instance != null)
        {
            Debug.LogWarning("There is more than one Save Manager in the scene");
            Destroy(gameObject);
        }

        else
        {
            instance = this;

        }

        LoadGame();
    }

    private void Update()
    {
#if DEBUG
        if (PlayerInputHandler.instance.DebugSaveInput.WasPressedThisFrame())
        {
            SaveGame();
        }

        if (PlayerInputHandler.instance.DebugDeleteSaveInput.WasPerformedThisFrame() && PlayerInputHandler.instance.DebugDeleteSaveInput2.WasPressedThisFrame())
        {
            DeleteSave();
        }
#endif
    }

    public void SaveGame()
    {
        if (SceneManager.GetActiveScene().name != "MainMenu" && SceneManager.GetActiveScene().name != "Storybook")
        {
            CollectDataForSaving();
            GameData dataToSave = new GameData();

            dataToSave.questData = gameData.questData;
            dataToSave.abilityData = gameData.abilityData;

            if (SceneManager.GetActiveScene().name == "Overworld")
            {
                dataToSave.playerPositionData = gameData.playerPositionData;
            }

            dataToSave.treeOfLifeState = gameData.treeOfLifeState;
            dataToSave.dialogueVariableData = gameData.dialogueVariableData;

            string jsonData = JsonUtility.ToJson(dataToSave);
            File.WriteAllText(saveFilePath, jsonData);

            Debug.Log("Game saved.");
        }
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
        isSaving = true;
        CollectQuestData();
        CollectAbilityData();
        CollectPlayerPositionData();
        CollectTreeOfLifeState();
        CollectDialogueVariableData();
        isSaving=false;
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
        if(SceneManager.GetActiveScene().name == SceneManagerHelper.GetSceneName(SceneManagerHelper.Scene.Overworld) || SceneManager.GetActiveScene().name == "OverWorld - VS")
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

    // this is probably causing the issues that teleports the player inside the dungeon entrances, making a loop between scene transitions
    // uncommenting this for now, if we want to use exact position for saving the data, we might need to think of other options /Jutta
    public void CollectPlayerPositionData()
    {
        if (FoxMovement.instance != null && SceneManager.GetActiveScene().name.Contains("Overworld"))
        {
            //gameData.playerPositionData = FoxMovement.instance.CollectPlayerPositionForSaving();
            gameData.playerPositionData = RespawnManager.instance.GetLatestRespawnPoint();
        }
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
    public Dictionary<Abilities, bool> GetLoadedAbilityDictionary()
    {
        Dictionary<Abilities, bool> loadedDictionary = new Dictionary<Abilities, bool>();

        if (File.Exists(saveFilePath))
        {
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

    public PositionData GetLoadedPlayerPosition()
    {
        //fetch the saved data from the file if there is a previous save, else it uses default starting position from the GameData/PositionData class

            PositionData data = gameData.playerPositionData;
            Debug.Log("SM loadplayerpos data: " + data);

            return data;
        
    }
    #endregion

    public void DeleteSave()
    {
        File.Delete(saveFilePath);
        gameData = new GameData();
        gameData.playerPositionData=new PositionData();

        Debug.Log("Save file deleted.");
    }
}