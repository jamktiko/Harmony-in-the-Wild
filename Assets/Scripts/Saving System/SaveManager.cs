using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    private string saveFilePath;

    public static SaveManager instance;

    private List<string> questData;
    private List<string> abilityData;

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

        dataToSave.questData = questData;

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

            questData = loadedData.questData;
        }
    }

    private void FetchDataForSaving()
    {
        questData = QuestManager.instance.CollectQuestDataForSaving();
        // abilityData = PlayerManager.instance.CollectAbilityData();
    }
    
    public List<string> FetchLoadedData(string dataType)
    {
        List<string> data = new List<string>();

        if (File.Exists(saveFilePath))
        {
            switch (dataType)
            {
                case "quest":
                    data = questData;
                    break;

                case "ability":
                    data = abilityData;
                    break;

                default:
                    Debug.LogWarning("No type match found when trying to load saved data for " + dataType);
                    break;
            }
        }

        return data;
    }
}
