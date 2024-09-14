using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public List<string> questData;
    public string abilityData;
    public PositionData playerPositionData;
    public int treeOfLifeState;
    public string dialogueVariableData;
}

[System.Serializable]
public class PositionData
{
    public float x;
    public float y;
    public float z;

    public float rotX;
    public float rotY;
    public float rotZ;
    public float rotW; // quaternion identity

    //default starting position
    public PositionData()
    {
        x = 1627f;
        y = 118f;
        z = 360f;

        rotX = 0;
        rotY = 0;
        rotZ = 0;
        rotW = 1;
    }

    public PositionData(Vector3 position, Quaternion rotation)
    {
        x = position.x;
        y = position.y;
        z = position.z;

        rotX = rotation.x;
        rotY = rotation.y;
        rotZ = rotation.z;
        rotW = rotation.w;
    }
}