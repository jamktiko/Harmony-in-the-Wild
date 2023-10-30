using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityToggling : MonoBehaviour
{
    [SerializeField] private int abilityIndex;

    private void Start()
    {
        GetComponent<Toggle>().isOn = PlayerManager.instance.abilityValues[abilityIndex];
    }

    public void ToggleAbility()
    {
        PlayerManager.instance.abilityValues[abilityIndex] = GetComponent<Toggle>().isOn;
    }
}
