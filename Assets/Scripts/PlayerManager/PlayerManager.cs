using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;

    [SerializeField] private int experience;
    [SerializeField] public int level;

    [Header("Abilities")]
    public List<bool> abilityValues;

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
        level = experience / 100;
        Debug.Log("Player leveled up to level "+level);
        return level;
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
