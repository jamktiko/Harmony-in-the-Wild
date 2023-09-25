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
    
    [Header("Abilities")]
    [SerializeField]
    public Dictionary<int,bool> abilityValues = new Dictionary<int, bool>()
    {
    { 1,false }, //Glider
    { 2,false }, //Swimming
    { 3,false }, //ChargeJump
    };

    public static PlayerManager instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("There is more than one Player Manager.");
        }

        instance = this;
    }

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

    public Dictionary<int, bool> CollectAbilityDataForSaving()
    {
        return abilityValues;
    }

    private void LoadAbilities()
    {
        abilityValues = SaveManager.instance.FetchLoadedAbilityData();
    }
}
