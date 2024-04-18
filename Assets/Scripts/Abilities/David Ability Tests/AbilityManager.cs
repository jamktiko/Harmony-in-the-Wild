using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using System.IO;
using Newtonsoft.Json;

public class AbilityManager : MonoBehaviour
{
    public static AbilityManager instance;

    public Dictionary<Abilities, bool> abilityStatuses = new Dictionary<Abilities, bool>();

    private Dictionary<Abilities, IAbility> abilities;
    public void Awake()
    {
        if (instance != null && instance != this)
        {
            Debug.LogWarning("There is more than one Ability Manager.");
            Destroy(gameObject);
            return;
        }

        instance = this;

        abilities = new Dictionary<Abilities, IAbility>();

        LoadAbilityData();
    }

    private void Update()
    {
        //For testing
        if (Input.GetKeyDown(KeyCode.L)) 
        {
            LogAbilityStatuses();
        }
        if (Input.GetKeyDown(KeyCode.U))
        {
            UnlockAbility(Abilities.Gliding);
        }
        if (Input.GetKeyDown(KeyCode.Y))
        {
            foreach (Abilities ability in Enum.GetValues(typeof(Abilities)))
            {
                abilityStatuses[ability] = true;
            }
        }
    }

    public void ActivateAbilityIfUnlocked(Abilities abilityType)
    {
        Debug.Log($"Tried activating ability: {abilityType}");

        if (abilityStatuses.TryGetValue(abilityType, out bool isUnlocked) && isUnlocked)
        {
            abilities[abilityType].Activate();
        }
        else
        {
            Debug.Log($"Abilities cannot be activated right now or {abilityType} is not unlocked.");
        }
    }

    public void RegisterAbility(Abilities abilityType, IAbility ability)
    {
        if (!abilities.ContainsKey(abilityType))
        {
            abilities.Add(abilityType, ability);
            Debug.Log($"Registered ability: {abilityType}");
        }
        else
        {
            Debug.Log($"Ability {abilityType} is already registered.");
        }
    }

    //Method to enable an ability
    public void UnlockAbility(Abilities abilityType)
    {
        if (abilityStatuses.ContainsKey(abilityType))
        {
            abilityStatuses[abilityType] = true;

            Debug.Log($"Ability {abilityType} has been unlocked.");
        }
        else
        {
            Debug.Log($"Attempted to unlock an unrecognized ability: {abilityType}");
        }
    }

    //For testing
    public void LogAbilityStatuses()
    {
        foreach (var ability in abilityStatuses)
        {
            string status = ability.Value ? "Unlocked" : "Locked";
            Debug.Log($"Ability: {ability.Key}, Status: {status}");
        }
    }

    public string CollectAbilityDataForSaving()
    {
        string data = "";
        data = JsonConvert.SerializeObject(abilityStatuses);

        Debug.Log("data: " + data);
        return data;
    }

    public void LoadAbilityData()
    {
        abilityStatuses = SaveManager.instance.LoadDictionaryFromJson();
    }
}
