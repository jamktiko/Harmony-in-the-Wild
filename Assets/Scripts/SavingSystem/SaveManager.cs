using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

//This script handles both saving and loading of gameData.
public class SaveManager : MonoBehaviour
{
    public EventHandler<Vector3> OnDungeonLoaded;

    public static SaveManager instance;

    private string saveFilePath;

    private GameData gameData = new GameData();

    private void Awake()
    {
        saveFilePath = Application.persistentDataPath + "/GameData.dat";

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

    public void GetPlayerOverworldCoordinates(Vector3 overworldPlayerPos)
    {
        OnDungeonLoaded?.Invoke(this, overworldPlayerPos);
    }

    public void SaveGame()
    {
        CollectDataForSaving();

        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(saveFilePath);
        GameData dataToSave = new GameData();

        dataToSave.questData = gameData.questData;
        dataToSave.abilityData = gameData.abilityData;

        string activeSceneName = SceneManager.GetActiveScene().name;

        string tutorialSceneName = SceneManagerHelper.GetSceneName(SceneManagerHelper.Scene.Tutorial);

        if (activeSceneName == tutorialSceneName)
        {
            dataToSave.playerPositionData= new List<float> { 1627f, 118f, 360f };
        }
        else
        {
            dataToSave.playerPositionData = gameData.playerPositionData;
        }
        bf.Serialize(file, dataToSave);

        file.Close();
    }

    private void LoadGame()
    {
        if (File.Exists(saveFilePath))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(saveFilePath, FileMode.Open);
            GameData loadedData = (GameData)bf.Deserialize(file);
            file.Close();

            gameData.questData = loadedData.questData;
            gameData.abilityData = loadedData.abilityData;
            gameData.playerPositionData = loadedData.playerPositionData;
        }
    }

    #region CollectDataForSaving
    private void CollectDataForSaving()
    {
        CollectQuestData();
        CollectAbilityData();
        CollectPlayerPositionData();
    }

    private void CollectQuestData()
    {
        gameData.questData = QuestManager.instance.CollectQuestDataForSaving();
    }

    private void CollectAbilityData()
    {
        gameData.abilityData = PlayerManager.instance.CollectAbilityDataForSaving();
    }
    private void CollectPlayerPositionData()
    {
        string activeSceneName = SceneManager.GetActiveScene().name;
        string overworldSceneName = SceneManagerHelper.GetSceneName(SceneManagerHelper.Scene.Overworld);

        Debug.Log(activeSceneName + " ; " + overworldSceneName);

        if (activeSceneName == overworldSceneName)
        {
            gameData.playerPositionData = FoxMovement.instance.CollectPlayerPositionForSaving();
            Debug.Log(gameData.playerPositionData);
        }
        else
        {
            //gameData.playerPositionData = new List<float> { 1627f, 118f, 360f };
            //Debug.Log("Default values were saved");
        }
    }
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

    public List<bool> GetLoadedAbilityData()
    {
        List<bool> data = new List<bool>();

        // fetch the saved data from the file if there is a previous save
        if (File.Exists(saveFilePath))
        {
            data = gameData.abilityData;
        }

        // if there isn't, return an empty list
        else
        {
            for (int i = 0; i < 8; i++)
            {
                data.Add(false);
            }
        }
        return data;
    }

    public List<float> GetLoadedPlayerPositionData()
    {
        List<float> data = new List<float>();

        // fetch the saved data from the file if there is a previous save
        if (File.Exists(saveFilePath))
        {
            data = gameData.playerPositionData;
        }

        // if there isn't, return default position
        else
        {
            data=new List<float> { 1627f,118f,360f };
        }

        return data;
    }
    #endregion

    private void DeleteSave()
    {
        File.Delete(saveFilePath);

        Debug.LogError("The save file has been deleted. Please restart the game to avoid any errors.");
    }
}