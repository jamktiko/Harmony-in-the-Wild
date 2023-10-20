using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DevToolButtons : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Comma))
        {
            transform.GetChild(0).gameObject.SetActive(!transform.GetChild(0).gameObject.activeInHierarchy);
        }
    }

    public void SaveAbilityChanges()
    {
        SaveManager.instance.SaveGame();
    }

    public void ResetAbilities()
    {
        for (int i = 0; i < 8; i++)
        {
            PlayerManager.instance.abilityValues[i] = false;
        }

        SaveManager.instance.SaveGame();
    }
}
