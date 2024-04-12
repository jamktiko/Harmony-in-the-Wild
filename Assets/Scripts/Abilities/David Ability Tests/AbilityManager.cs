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

    //public bool CanActivateAbilities { get; set; } = false;

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

        //InitializeAbilities();
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
            EnableAbility(Abilities.Gliding);
        }
        if (Input.GetKeyDown(KeyCode.Y))
        {
            foreach (Abilities ability in Enum.GetValues(typeof(Abilities)))
            {
                abilityStatuses[ability] = true;
            }
        }
    }

    //note: moved into the SaveManager 'cus it's cleaner and this doesn't have to be here
    //public void InitializeAbilities()
    //{
    //    //Initialize all abilities as false (disabled) by default
    //    foreach (Abilities ability in Enum.GetValues(typeof(Abilities)))
    //    {
    //        abilityStatuses.Add(ability, false);
    //    }
    //}

    public void TryActivateAbility(Abilities abilityType)
    {
        Debug.Log($"Tried activating ability: {abilityType}");

        if (abilityStatuses.TryGetValue(abilityType, out bool isEnabled) && isEnabled)
        {
            abilities[abilityType].Activate();
        }
        else
        {
            Debug.Log($"Abilities cannot be activated right now or {abilityType} is disabled.");
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
    public void EnableAbility(Abilities abilityType)
    {
        if (abilityStatuses.ContainsKey(abilityType))
        {
            abilityStatuses[abilityType] = true;

            Debug.Log($"Ability {abilityType} has been enabled.");
        }
        else
        {
            Debug.Log($"Attempted to enable an unrecognized ability: {abilityType}");
        }
    }

    //For testing
    public void LogAbilityStatuses()
    {
        foreach (var ability in abilityStatuses)
        {
            string status = ability.Value ? "Enabled" : "Disabled";
            Debug.Log($"Ability: {ability.Key}, Status: {status}");
        }
    }

    public string CollectAbilityDataForSaving()
    {
        //serialize this shit

        //AbilityData abilityData = new AbilityData();

        string data = "";

        //abilityData.serializedAbilityStatuses = abilityStatuses;

        // Create a new dictionary with string keys for serialization
        //Dictionary<string, bool> stringDictionary = new Dictionary<string, bool>();

        // Populate the stringDictionary with enum keys converted to strings
        //foreach (var kvp in abilityStatuses)
        //{
        //    string abilityKey = kvp.Key.ToString(); // Convert enum key to string
        //    bool abilityValue = kvp.Value;

        //    abilityData.serializedAbilityStatuses[abilityKey] = abilityValue;
        //}

        //foreach (var kvp2 in abilityData.serializedAbilityStatuses)
        //{
        //    Debug.Log($"CollectAbData: {kvp2.Key}: {kvp2.Value}");
        //}

        // Serialize the stringDictionary to JSON
        data = JsonConvert.SerializeObject(abilityStatuses);

        Debug.Log("data: " + data);
        return data;
    }

    public void LoadAbilityData()
    {
        abilityStatuses = SaveManager.instance.LoadDictionaryFromJson();
    }
}
