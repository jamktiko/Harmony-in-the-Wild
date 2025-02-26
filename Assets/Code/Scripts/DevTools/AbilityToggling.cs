using UnityEngine;

public class AbilityToggling : MonoBehaviour
{
    [SerializeField] Abilities abilities;

    //note: REDUNDANT replaced with AbilityManager
    private void Start()
    {
        AbilityManager.Instance.UnlockAbility(Abilities.ChargeJumping);
    }

    //public void ToggleAbility()
    //{
    //    PlayerManager.instance.hasAbilityValues[abilityIndex] = GetComponent<Toggle>().isOn;
    //}
}
