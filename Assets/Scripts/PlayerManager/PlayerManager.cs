using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] private int experience;
    [SerializeField] public int Level;

    [Header("Abilities")]
    public List<bool> abilityValues;

    public static PlayerManager instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("There is more than one Player Manager.");
            Destroy(gameObject);
        }
        instance = this;

        LoadAbilities();
    }

    public void GetAbility(int index)
    {
        abilityValues[index] = true;

        if(index == 5)
        {
            GameEventsManager.instance.playerEvents.GhostSpeakActivated();
        }
        
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
