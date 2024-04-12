using UnityEngine;
using UnityEngine.UI;

public class AbilityToggling : MonoBehaviour
{
    [SerializeField] private int abilityIndex;

    //note: REDUNDANT replaced with AbilityManager
    //private void Start()
    //{
    //    GetComponent<Toggle>().isOn = PlayerManager.instance.hasAbilityValues[abilityIndex];
    //}

    //public void ToggleAbility()
    //{
    //    PlayerManager.instance.hasAbilityValues[abilityIndex] = GetComponent<Toggle>().isOn;
    //}
}
