using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

//This script handles both saving and loading of gameData.

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance;

    [FormerlySerializedAs("saveFilePath")] public string SaveFilePath;

    [FormerlySerializedAs("gameData")] public GameData GameData = new GameData();

    public static bool IsSaving;

    private void Awake()
    {
        SaveFilePath = Application.persistentDataPath + "/GameData.json";

        if (SaveManager.Instance != null)
        {
            Debug.LogWarning("There is more than one Save Manager in the scene");
            Destroy(gameObject);
        }

        else
        {
            Instance = this;

        }

        LoadGame();
    }

    private void Update()
    {
#if DEBUG
        if (PlayerInputHandler.Instance.DebugSaveInput.WasPressedThisFrame())
        {
            SaveGame();
        }

        if (PlayerInputHandler.Instance.DebugDeleteSaveInput.WasPerformedThisFrame() && PlayerInputHandler.Instance.DebugDeleteSaveInput2.WasPressedThisFrame())
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

            dataToSave.QuestData = GameData.QuestData;
            dataToSave.AbilityData = GameData.AbilityData;

            if (SceneManager.GetActiveScene().name == "Overworld" || SceneManager.GetActiveScene().name == "OverWorld - VS")
            {
                dataToSave.PlayerPositionData = GameData.PlayerPositionData;
                Debug.Log("Saving player position in Overworld...");
            }

            dataToSave.TreeOfLifeState = GameData.TreeOfLifeState;
            dataToSave.DialogueVariableData = GameData.DialogueVariableData;
            dataToSave.ActiveQuest = QuestManager.Instance.GetActiveQuest();

            dataToSave.BerryCollectibles = GameData.BerryCollectibles;
            dataToSave.PineconeCollectibles = GameData.PineconeCollectibles;

            dataToSave.BerryData = GameData.BerryData;
            dataToSave.PineconeData = GameData.PineconeData;

            string jsonData = JsonUtility.ToJson(dataToSave);
            File.WriteAllText(SaveFilePath, jsonData);

            Debug.Log("Game saved.");
        }
    }
    private void LoadGame()
    {
        if (File.Exists(SaveFilePath))
        {
            string jsonData = File.ReadAllText(SaveFilePath);
            GameData loadedData = JsonUtility.FromJson<GameData>(jsonData);

            GameData.QuestData = loadedData.QuestData;
            GameData.AbilityData = loadedData.AbilityData;

            GameData.PlayerPositionData = loadedData.PlayerPositionData;

            GameData.TreeOfLifeState = loadedData.TreeOfLifeState;
            GameData.DialogueVariableData = loadedData.DialogueVariableData;

            GameData.BerryCollectibles = loadedData.BerryCollectibles;
            GameData.PineconeCollectibles = loadedData.PineconeCollectibles;

            GameData.BerryData = loadedData.BerryData;
            GameData.PineconeData = loadedData.PineconeData;

            GameData.ActiveQuest = loadedData.ActiveQuest;
            GameEventsManager.instance.QuestEvents.ChangeActiveQuest(GameData.ActiveQuest);

            Debug.Log("Game loaded.");
        }
    }
    #region CollectDataForSaving
    private void CollectDataForSaving()
    {
        IsSaving = true;
        CollectQuestData();
        CollectAbilityData();
        CollectPlayerPositionData();
        CollectTreeOfLifeState();
        CollectDialogueVariableData();
        CollectBerryCollectibleData();
        CollectPineConeCollectibleData();
        IsSaving = false;
    }

    private void CollectQuestData()
    {
        GameData.QuestData = QuestManager.Instance.CollectQuestDataForSaving();
    }

    private void CollectAbilityData()
    {
        GameData.AbilityData = AbilityManager.Instance.CollectAbilityDataForSaving();
    }

    private void CollectTreeOfLifeState()
    {
        if (SceneManager.GetActiveScene().name == SceneManagerHelper.GetSceneName(SceneManagerHelper.Scene.Overworld) || SceneManager.GetActiveScene().name == "OverWorld - VS")
        {
            GameData.TreeOfLifeState = TreeOfLifeState.Instance.GetTreeOfLifeState();
        }

        else
        {
            Debug.Log("No ToL state fetched, not in Overworld.");
        }
    }

    private void CollectDialogueVariableData()
    {
        GameData.DialogueVariableData = DialogueManager.Instance.CollectDialogueVariableDataForSaving();
    }

    public int GetTreeOfLifeState()
    {
        return GameData.TreeOfLifeState;
    }

    // this is probably causing the issues that teleports the player inside the dungeon entrances, making a loop between scene transitions
    // uncommenting this for now, if we want to use exact position for saving the data, we might need to think of other options /Jutta
    public void CollectPlayerPositionData()
    {
        if (FoxMovement.Instance != null && (SceneManager.GetActiveScene().name.Contains("Overworld") || SceneManager.GetActiveScene().name.Contains("OverWorld")))
        {
            //gameData.playerPositionData = FoxMovement.instance.CollectPlayerPositionForSaving();
            GameData.PlayerPositionData = RespawnManager.Instance.GetLatestRespawnPoint();
            Debug.Log("Saving player position: " + GameData.PlayerPositionData.X + ", " + GameData.PlayerPositionData.Y + ", " + GameData.PlayerPositionData.Z);
        }
    }

    public void CollectBerryCollectibleData()
    {
        GameData.BerryData = PlayerManager.Instance.CollectBerryDataForSaving();
    }

    public void CollectPineConeCollectibleData()
    {
        GameData.PineconeData = PlayerManager.Instance.CollectPineconeDataForSaving();
    }

    #endregion

    #region GetDataForLoading
    public List<string> GetLoadedQuestData(string dataType)
    {
        List<string> data = new List<string>();

        if (File.Exists(SaveFilePath))
        {
            switch (dataType)
            {
                case "quest":
                    data = GameData.QuestData;
                    break;

                default:
                    Debug.LogWarning("No type match found when trying to load saved data for " + dataType);
                    break;
            }
        }

        return data;
    }

    public Dictionary<string, bool> GetLoadedBerryDictionary()
    {
        Dictionary<string, bool> loadedDictionary = new Dictionary<string, bool>();

        loadedDictionary = JsonConvert.DeserializeObject<Dictionary<string, bool>>(GameData.BerryData);

        return loadedDictionary;
    }

    public Dictionary<string, bool> GetLoadedPineConeDictionary()
    {
        Dictionary<string, bool> loadedDictionary = new Dictionary<string, bool>();

        loadedDictionary = JsonConvert.DeserializeObject<Dictionary<string, bool>>(GameData.PineconeData);

        return loadedDictionary;
    }

    public Dictionary<Abilities, bool> GetLoadedAbilityDictionary()
    {
        Dictionary<Abilities, bool> loadedDictionary = new Dictionary<Abilities, bool>();

        if (File.Exists(SaveFilePath))
        {
            loadedDictionary = JsonConvert.DeserializeObject<Dictionary<Abilities, bool>>(GameData.AbilityData);
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
        return GameData.DialogueVariableData;
    }

    public PositionData GetLoadedPlayerPosition()
    {
        //fetch the saved data from the file if there is a previous save, else it uses default starting position from the GameData/PositionData class

        PositionData data = GameData.PlayerPositionData;
        Debug.Log("SM loadplayerpos data: " + data);

        return data;

    }
    #endregion

    public void DeleteSave()
    {
        File.Delete(SaveFilePath);
        GameData = new GameData();
        GameData.PlayerPositionData = new PositionData();

        Debug.Log("Save file deleted.");
    }
}