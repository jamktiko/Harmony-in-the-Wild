using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableCounter : MonoBehaviour
{
    public int Counter = 0;
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
}
