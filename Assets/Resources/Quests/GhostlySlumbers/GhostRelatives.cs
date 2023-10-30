using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostRelatives : MonoBehaviour
{
    public static GhostRelatives instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("There is more than one Ghost Relatives.");
        }

        instance = this;
    }

    public List<GameObject> GetGhostRelatives()
    {
        List<GameObject> relatives = new List<GameObject>();

        foreach(Transform child in transform)
        {
            relatives.Add(child.gameObject);
        }

        return relatives;
    }
}
