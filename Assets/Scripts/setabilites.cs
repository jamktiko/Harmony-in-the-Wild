using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetAbilites : MonoBehaviour
{
    void Start()
    {
        for (int i = 0; i < PlayerManager.instance.hasAbilityValues.Count; i++)
        {
            PlayerManager.instance.hasAbilityValues[i] = false;
        }
        SaveManager.instance.SaveGame();
    }
}
