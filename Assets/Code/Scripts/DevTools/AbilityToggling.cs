using UnityEngine;
using UnityEngine.UI;

public class AbilityToggling : MonoBehaviour
{
    [SerializeField] Abilities abilities;

    //note: REDUNDANT replaced with AbilityManager
    private void Start()
    {
        AbilityManager.instance.UnlockAbility(Abilities.ChargeJumping);
    }

    //public void ToggleAbility()
    //{
    //    PlayerManager.instance.hasAbilityValues[abilityIndex] = GetComponent<Toggle>().isOn;
    //}
}
