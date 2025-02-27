using UnityEngine;

public class VerticalSlicePenguin : MonoBehaviour
{
    private void Start()
    {
        AbilityManager.Instance.UnlockAbility(Abilities.RockDestroying);
        AbilityManager.Instance.UnlockAbility(Abilities.SnowDiving);
        AbilityManager.Instance.UnlockAbility(Abilities.Freezing);
        AbilityManager.Instance.LockAbility(Abilities.Gliding);
        AbilityCycle.Instance.SelectedAbility = Abilities.Freezing;
    }

}
