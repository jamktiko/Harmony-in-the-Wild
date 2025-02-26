using System.Collections.Generic;

[System.Serializable]
public class AbilityData
{
    public Dictionary<Abilities, bool> SerializedAbilityStatuses;

    public AbilityData()
    {
        SerializedAbilityStatuses = new Dictionary<Abilities, bool>();
    }
}
