using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AbilityTestingTools : MonoBehaviour
{
#if DEBUG
    [Header(" K = check one \n L = check all \n U = unlock one \n I = unlock all")]
    public Abilities abilityToUnlock;

    private void Update()
    {
        //check one
        if (PlayerInputHandler.instance.DebugAbilitiesCheckOne.WasPressedThisFrame())
        {
            LogAbilityStatus();
        }

        //check all
        if (PlayerInputHandler.instance.DebugAbilitiesCheckAll.WasPressedThisFrame())
        {
            LogAllAbilityStatuses();
        }

        //unlock one
        if (PlayerInputHandler.instance.DebugAbilitiesUnlockOne.WasPressedThisFrame())
        {
            AbilityManager.instance.UnlockAbility(abilityToUnlock);
        }

        //unlock all
        if (PlayerInputHandler.instance.DebugAbilitiesUnlockAll.WasPressedThisFrame())
        {
            foreach (Abilities ability in Enum.GetValues(typeof(Abilities)))
            {
                AbilityManager.instance.abilityStatuses[ability] = true;
            }

            Debug.Log("All abilities unlocked");
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
#endif
}
