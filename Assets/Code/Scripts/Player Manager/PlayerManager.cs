using Ink.Runtime;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;

    [SerializeField] private int experience;
    [SerializeField] private int level;
    [SerializeField] public int Berries;
    [SerializeField] public int PineCones;
    [SerializeField] public Dictionary<string, bool> BerryData =new Dictionary<string, bool>();
    [SerializeField] public Dictionary<string, bool> PineConeData = new Dictionary<string, bool>();
    private void Awake()
    {
        if (PlayerManager.instance != null)
        {
            Debug.LogWarning("There is more than one Player Manager in the scene");
            Destroy(gameObject);
        }

        else
        {
            instance = this;

        }
    }
    public void GenerateCollectibleData()
    {
        BerryData = SaveManager.instance.GetLoadedBerryDictionary();
        Berries=BerryData.Where(x => !x.Value).Count();
        PineConeData = SaveManager.instance.GetLoadedPineConeDictionary();
        PineCones = PineConeData.Where(x => !x.Value).Count();
    }
    public int LevelCheck() 
    {
        level = experience / 100;
        Debug.Log("Player leveled up to level "+level);
        return level;
    }

    public string CollectBerryDataForSaving() 
    {
        string data = JsonConvert.SerializeObject(BerryData);

        return data;
    }

    public string CollectPineconeDataForSaving() 
    {
        string data = JsonConvert.SerializeObject(PineConeData);

        return data;
    }
    //public Vector3 GetDefaultPlayerPosition()
    //{
    //    return defaultPlayerPosition;
    //}
}
