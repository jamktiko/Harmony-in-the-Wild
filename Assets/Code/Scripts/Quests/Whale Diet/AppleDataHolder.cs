using System.Collections.Generic;
using UnityEngine;

public class AppleDataHolder : MonoBehaviour
{
    public static AppleDataHolder Instance;

    private List<GameObject> _apples = new List<GameObject>();

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogWarning("There is more than one AppleDataHolder in the scene!");
            Destroy(this);
        }

        else
        {
            Instance = this;
        }
    }

    public List<GameObject> GetApples()
    {
        // initialize the apple list
        foreach (Transform child in transform)
        {
            _apples.Add(child.gameObject);
        }

        return _apples;
    }
}
