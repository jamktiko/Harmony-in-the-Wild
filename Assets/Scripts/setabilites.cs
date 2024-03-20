using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class setabilites : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < PlayerManager.instance.hasAbilityValues.Count; i++)
        {
            PlayerManager.instance.hasAbilityValues[i] = false;
        }
        SaveManager.instance.SaveGame();
    }
}
