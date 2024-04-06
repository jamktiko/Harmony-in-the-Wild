using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

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

        InitializeAbilities();
    }

    private void Update()
    {
        //For testing
        if (Input.GetKeyDown(KeyCode.L)) 
        {
            EnableAbility(Abilities.Gliding);

            LogAbilityStatuses();
        }
    }

    private void InitializeAbilities()
    {
        //Initialize all abilities as false (disabled) by default
        foreach (Abilities ability in Enum.GetValues(typeof(Abilities)))
        {
            abilityStatuses.Add(ability, false);
        }
    }

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
    public Dictionary<Abilities, bool> CollectAbilityDataForSaving()
    {
        return abilityStatuses;
    }
}
