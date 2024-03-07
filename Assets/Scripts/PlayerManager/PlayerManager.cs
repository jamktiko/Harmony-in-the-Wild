using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;

    [SerializeField] private int experience;
    [SerializeField] private int level;

    [Header("Abilities")]
    public List<bool> hasAbilityValues; //NOTE: Make private and allow access through methods?

    private Vector3 defaultPlayerPosition = new Vector3(1627f, 118f, 360f);

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
        hasAbilityValues[index] = true;

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
        return hasAbilityValues;
    }

    private void LoadAbilities()
    {
        hasAbilityValues = SaveManager.instance.GetLoadedAbilityData();
    }

    public Vector3 GetDefaultPlayerPosition()
    {
        return defaultPlayerPosition;
    }
}
