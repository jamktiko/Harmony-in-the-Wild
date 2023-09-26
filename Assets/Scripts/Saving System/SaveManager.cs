using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    private string saveFilePath;

    public static SaveManager instance;

    private GameData gameData = new GameData();

    private void Awake()
    {
        // NOTE CHANGE THIS TO .DAT LATER!!
        saveFilePath = Application.persistentDataPath + "/gameData.txt";

        if(instance != null)
        {
            Debug.LogWarning("There is more than one Save Manager.");
        }

        instance = this;

        LoadGame();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            SaveGame();
        }
    }

    private void SaveGame()
    {
        FetchDataForSaving();

        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(saveFilePath);
        GameData dataToSave = new GameData();

        dataToSave.questData = gameData.questData;
        dataToSave.abilityData = gameData.abilityData;

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
        }
    }

    private void FetchDataForSaving()
    {
        gameData.questData = QuestManager.instance.CollectQuestDataForSaving();
        gameData.abilityData = PlayerManager.instance.CollectAbilityDataForSaving();
    }
    
    public List<string> FetchLoadedData(string dataType)
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

    public List<bool> FetchLoadedAbilityData()
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
}
