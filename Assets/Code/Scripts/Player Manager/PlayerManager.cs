using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

[System.Serializable]
public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance;

    [FormerlySerializedAs("experience")] [SerializeField] private int _experience;
    [FormerlySerializedAs("level")] [SerializeField] private int _level;
    [SerializeField] public int Berries;
    [SerializeField] public int PineCones;
    [SerializeField] public Dictionary<string, bool> BerryData = new Dictionary<string, bool>();
    [SerializeField] public Dictionary<string, bool> PineConeData = new Dictionary<string, bool>();
    private void Awake()
    {
        if (PlayerManager.Instance != null)
        {
            Debug.LogWarning("There is more than one Player Manager in the scene");
            Destroy(gameObject);
        }

        else
        {
            Instance = this;

        }
    }
    public void GenerateCollectibleData()
    {
        BerryData = SaveManager.Instance.GetLoadedBerryDictionary();
        Berries = BerryData.Where(x => !x.Value).Count();
        PineConeData = SaveManager.Instance.GetLoadedPineConeDictionary();
        PineCones = PineConeData.Where(x => !x.Value).Count();
    }
    public int LevelCheck()
    {
        _level = _experience / 100;
        Debug.Log("Player leveled up to level " + _level);
        return _level;
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
