using System.Collections.Generic;
using UnityEngine;

public class GhostRelatives : MonoBehaviour
{
    public static GhostRelatives Instance;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogWarning("There is more than one Ghost Relatives.");
        }

        Instance = this;
    }

    public List<GameObject> GetGhostRelatives()
    {
        List<GameObject> relatives = new List<GameObject>();

        foreach (Transform child in transform)
        {
            relatives.Add(child.gameObject);
        }

        return relatives;
    }
}
