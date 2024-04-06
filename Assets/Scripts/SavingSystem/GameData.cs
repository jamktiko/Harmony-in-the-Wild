using System.Collections.Generic;

[System.Serializable]
public class GameData
{
    public List<string> questData;
    public Dictionary<Abilities, bool> abilityData;
    public List<float> playerPositionData;
}
