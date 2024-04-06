using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AbilityData
{
    public Dictionary<Abilities, bool> serializedAbilityStatuses;

    public AbilityData()
    {
        serializedAbilityStatuses = new Dictionary<Abilities, bool>();
    }
}
