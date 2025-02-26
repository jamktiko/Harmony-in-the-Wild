using System.Collections.Generic;
using UnityEngine;

public class AppleDataHolder : MonoBehaviour
{
    public static AppleDataHolder instance;

    private List<GameObject> apples = new List<GameObject>();

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("There is more than one AppleDataHolder in the scene!");
            Destroy(this);
        }

        else
        {
            instance = this;
        }
    }

    public List<GameObject> GetApples()
    {
        // initialize the apple list
        foreach (Transform child in transform)
        {
            apples.Add(child.gameObject);
        }

        return apples;
    }
}
