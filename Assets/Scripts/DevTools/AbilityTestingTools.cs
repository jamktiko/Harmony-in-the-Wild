using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AbilityTestingTools : MonoBehaviour
{
    [Header(" K = check one \n L = check all \n U = unlock one \n Y = unlock all")]
    public Abilities abilityToUnlock;

    private void Update()
    {
        //check one
        if (Input.GetKeyDown(KeyCode.K))
        {
            LogAbilityStatus();
        }

        //check all
        if (Input.GetKeyDown(KeyCode.L))
        {
            LogAllAbilityStatuses();
        }

        //unlock one
        if (Input.GetKeyDown(KeyCode.U))
        {
            AbilityManager.instance.UnlockAbility(abilityToUnlock);
        }

        //unlock all
        if (Input.GetKeyDown(KeyCode.Y))
        {
            foreach (Abilities ability in Enum.GetValues(typeof(Abilities)))
            {
                AbilityManager.instance.abilityStatuses[ability] = true;
            }
        }
    }

    private void LogAbilityStatus()
    {
        if (AbilityManager.instance.abilityStatuses.TryGetValue(abilityToUnlock, out bool isUnlocked))
        {
            string status = isUnlocked ? "Unlocked" : "Locked";
            Debug.Log($"Ability: {abilityToUnlock}, Status: {status}");
        }
        if (AbilityCycle.instance.activeAbilities.TryGetValue(AbilityCycle.instance.selectedAbility, out bool isActive))
        {
            string status = isActive ? "Active" : "Inactive";
            Debug.Log("1. Selected ability is: " + AbilityCycle.instance.selectedAbility + " and it is: " + status);
        }
    }

    public void LogAllAbilityStatuses()
    {
        foreach (var ability in AbilityManager.instance.abilityStatuses)
        {
            string status = ability.Value ? "Unlocked" : "Locked";
            Debug.Log($"Ability: {ability.Key}, Status: {status}");
        }
    }
}
