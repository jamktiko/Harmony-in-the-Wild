using UnityEngine;
using UnityEngine.Serialization;

public class AbilityToggling : MonoBehaviour
{
    [FormerlySerializedAs("abilities")] [SerializeField] Abilities _abilities;

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
