using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AbilityData
{
    public Dictionary<string, bool> serializedAbilityStatuses;

    public AbilityData()
    {
        serializedAbilityStatuses = new Dictionary<string, bool>();
    }
}
