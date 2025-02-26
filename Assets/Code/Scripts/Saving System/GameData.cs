using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[System.Serializable]
public class GameData
{
    [FormerlySerializedAs("questData")] public List<string> QuestData;
    [FormerlySerializedAs("activeQuest")] public string ActiveQuest;
    [FormerlySerializedAs("abilityData")] public string AbilityData;
    [FormerlySerializedAs("playerPositionData")] public PositionData PlayerPositionData;
    [FormerlySerializedAs("treeOfLifeState")] public int TreeOfLifeState;
    [FormerlySerializedAs("dialogueVariableData")] public string DialogueVariableData;
    public int BerryCollectibles;
    [FormerlySerializedAs("berryData")] public string BerryData;
    public int PineconeCollectibles;
    public string PineconeData;
}

[System.Serializable]
public class PositionData
{
    [FormerlySerializedAs("x")] public float X;
    [FormerlySerializedAs("y")] public float Y;
    [FormerlySerializedAs("z")] public float Z;

    [FormerlySerializedAs("rotX")] public float RotX;
    [FormerlySerializedAs("rotY")] public float RotY;
    [FormerlySerializedAs("rotZ")] public float RotZ;
    [FormerlySerializedAs("rotW")] public float RotW; // quaternion identity

    //default starting position
    public PositionData()
    {
        X = 219f;
        Y = 103f;
        Z = 757f;

        RotX = 0;
        RotY = 0;
        RotZ = 0;
        RotW = 1;
    }

    public PositionData(Vector3 position, Quaternion rotation)
    {
        X = position.x;
        Y = position.y;
        Z = position.z;

        RotX = rotation.x;
        RotY = rotation.y;
        RotZ = rotation.z;
        RotW = rotation.w;
    }
}