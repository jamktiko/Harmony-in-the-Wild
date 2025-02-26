using System.Collections.Generic;
using UnityEngine;

public class UnstuckDataBank : MonoBehaviour
{
    public static UnstuckDataBank instance;

    [SerializeField] private Transform unstuckRescuePointParent;
    [SerializeField] private Transform player;

    private List<Transform> rescuePoints = new List<Transform>();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        else
        {
            Debug.LogWarning("Multiple Unstuck Data Banks in the scene!");
        }

        CreateRescuePointList();
    }

    private void CreateRescuePointList()
    {
        foreach (Transform child in unstuckRescuePointParent)
        {
            rescuePoints.Add(child);
        }
    }

    public List<Transform> GetRescuePoints()
    {
        return rescuePoints;
    }

    public Transform GetPlayer()
    {
        return player;
    }
}
