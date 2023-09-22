using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] private int experience;
    [SerializeField] public int Level;
    
    //public struct abilityData
    //{
    //    public int abilityIndex;
    //    public string abilityName;
    //    public bool activated;
    //    public abilityData(int index, string name)
    //    {
    //        abilityIndex = index;
    //        abilityName = name;
    //        activated = false;
    //    }
    //}
    [Header("Abilities")]
    [SerializeField]
    Dictionary<int,bool> abilityValues = new Dictionary<int, bool>()
    {
    { 1,false }, //Glider
    { 2,false }, //Swimming
    { 3,false }, //ChargeJump
    };
//public List<abilityData> abilityDatas = new List<abilityData>();

//private void Start()
//{
//    string path = Application.dataPath + "/Data/abilityData.txt";
//    string[] lines = File.ReadAllLines(path);
//    foreach (var line in lines)
//    {
//        string[] oneLine = line.Split();
//        abilityDatas.Add(new abilityData(int.Parse(oneLine[0]), oneLine[1]));
//    }
//}
private void OnEnable()
    {
        GameEventsManager.instance.playerEvents.onExperienceGained += ExperienceGained;

    }

    private void OnDisable()
    {
        GameEventsManager.instance.playerEvents.onExperienceGained -= ExperienceGained;
    }

    private void ExperienceGained(int newExperience)
    {
        experience += newExperience;
        GameEventsManager.instance.playerEvents.ExperienceChanged(experience);

        Debug.Log("Current experience: " + experience);
        LevelCheck();
        getAbility(1);
    }
    public void getAbility(int index)
    {
        abilityValues[index] = true;
        
    }
    public int LevelCheck() 
    {
        Level = experience / 100;
        Debug.Log("Player leveled up to level "+Level);
        return Level;
    }
}
