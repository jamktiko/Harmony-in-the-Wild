using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VerticalSlicePenguin : MonoBehaviour
{
    void Start()
    {
        AbilityManager.instance.UnlockAbility(Abilities.RockDestroying);
        AbilityManager.instance.UnlockAbility(Abilities.SnowDiving);
        AbilityManager.instance.UnlockAbility(Abilities.Freezing);
        AbilityCycle.instance.selectedAbility = Abilities.Freezing;
    }

}
