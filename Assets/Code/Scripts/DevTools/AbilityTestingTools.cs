using System;
using UnityEngine;
using UnityEngine.Serialization;

public class AbilityTestingTools : MonoBehaviour
{
#if DEBUG
    [FormerlySerializedAs("abilityToUnlock")] [Header(" K = check one \n L = check all \n U = unlock one \n I = unlock all")]
    public Abilities AbilityToUnlock;

    private void Update()
    {
        //check one
        if (PlayerInputHandler.Instance.DebugAbilitiesCheckOne.WasPressedThisFrame())
        {
            LogAbilityStatus();
        }

        //check all
        if (PlayerInputHandler.Instance.DebugAbilitiesCheckAll.WasPressedThisFrame())
        {
            LogAllAbilityStatuses();
        }

        //unlock one
        if (PlayerInputHandler.Instance.DebugAbilitiesUnlockOne.WasPressedThisFrame())
        {
            AbilityManager.Instance.UnlockAbility(AbilityToUnlock);
        }

        //unlock all
        if (PlayerInputHandler.Instance.DebugAbilitiesUnlockAll.WasPressedThisFrame())
        {
            foreach (Abilities ability in Enum.GetValues(typeof(Abilities)))
            {
                AbilityManager.Instance.AbilityStatuses[ability] = true;
            }

            Debug.Log("All abilities unlocked");
        }
    }

    private void LogAbilityStatus()
    {
        if (AbilityManager.Instance.AbilityStatuses.TryGetValue(AbilityToUnlock, out bool isUnlocked))
        {
            string status = isUnlocked ? "Unlocked" : "Locked";
            Debug.Log($"Ability: {AbilityToUnlock}, Status: {status}");
        }
        if (AbilityCycle.Instance.ActiveAbilities.TryGetValue(AbilityCycle.Instance.SelectedAbility, out bool isActive))
        {
            string status = isActive ? "Active" : "Inactive";
            Debug.Log("1. Selected ability is: " + AbilityCycle.Instance.SelectedAbility + " and it is: " + status);
        }
    }

    public void LogAllAbilityStatuses()
    {
        foreach (var ability in AbilityManager.Instance.AbilityStatuses)
        {
            string status = ability.Value ? "Unlocked" : "Locked";
            Debug.Log($"Ability: {ability.Key}, Status: {status}");
        }
    }
#endif
}
