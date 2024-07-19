using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VerticalSlicePenguin : MonoBehaviour
{
    void Start()
    {
        AbilityManager.instance.UnlockAbility(Abilities.Freezing);
        AbilityManager.instance.UnlockAbility(Abilities.RockDestroying);
        AbilityManager.instance.UnlockAbility(Abilities.SnowDiving);
        AbilityCycle.instance.selectedAbility = Abilities.Freezing;
    }

}
