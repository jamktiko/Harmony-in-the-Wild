using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyOnLoadMenu : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
}
