using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveManager : MonoBehaviour
{
    public static SaveManager instance;

    private string saveFilePath;

    private GameData gameData = new GameData();

    private void Awake()
    {
        saveFilePath = Application.persistentDataPath + "/gameData.dat";

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
        if (Input.GetKeyDown(KeyCode.O))
        {
            SaveGame();
        }

        if (Input.GetKey(KeyCode.X) && Input.GetKeyDown(KeyCode.Z))
        {
            DeleteSave();
        }
    }

    public void SaveGame()
    {
        FetchDataForSaving();

        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(saveFilePath);
        GameData dataToSave = new GameData();

        dataToSave.questData = gameData.questData;
        dataToSave.abilityData = gameData.abilityData;
        if (SceneManager.GetActiveScene()==SceneManager.GetSceneByBuildIndex(12))
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

    private void FetchDataForSaving()
    {
        gameData.questData = QuestManager.instance.CollectQuestDataForSaving();
        gameData.abilityData = PlayerManager.instance.CollectAbilityDataForSaving();
        if (SceneManager.GetActiveScene()==SceneManager.GetSceneByBuildIndex(3))
        {
            gameData.playerPositionData = FoxMovement.instance.CollectPlayerPositionForSaving();
        }
        else
        {
            gameData.playerPositionData = new List<float> { 1627f, 118f, 360f };
        }
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
    public List<float> FetchLoadedPlayerPositionData()
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

    private void DeleteSave()
    {
        File.Delete(saveFilePath);

        Debug.LogError("The save file has been deleted. Please restart the game to avoid any errors.");
    }
}
