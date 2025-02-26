using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class UnstuckDataBank : MonoBehaviour
{
    public static UnstuckDataBank Instance;

    [FormerlySerializedAs("unstuckRescuePointParent")] [SerializeField] private Transform _unstuckRescuePointParent;
    [FormerlySerializedAs("player")] [SerializeField] private Transform _player;

    private List<Transform> _rescuePoints = new List<Transform>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        else
        {
            Debug.LogWarning("Multiple Unstuck Data Banks in the scene!");
        }

        CreateRescuePointList();
    }

    private void CreateRescuePointList()
    {
        foreach (Transform child in _unstuckRescuePointParent)
        {
            _rescuePoints.Add(child);
        }
    }

    public List<Transform> GetRescuePoints()
    {
        return _rescuePoints;
    }

    public Transform GetPlayer()
    {
        return _player;
    }
}
