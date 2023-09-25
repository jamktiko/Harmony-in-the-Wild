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
    public List<bool> abilityValues = new List<bool>();

    public static PlayerManager instance;

    private void Awake()
    {
        for (int i = 0; i < 8; i++) 
        {
            abilityValues.Add(false);
        }
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

    public List<bool> CollectAbilityDataForSaving()
    {
        return abilityValues;
    }

    private void LoadAbilities()
    {
        abilityValues = SaveManager.instance.FetchLoadedAbilityData();
    }
}
