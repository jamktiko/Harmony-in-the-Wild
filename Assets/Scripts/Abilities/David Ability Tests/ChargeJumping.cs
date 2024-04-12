using UnityEngine;

public class ChargeJumping : MonoBehaviour, IAbility
{
    private void Start()
    {
        AbilityManager.instance.RegisterAbility(Abilities.ChargeJumping, this);
    }

    public void Activate()
    {
        Debug.Log("ChargeJumping activated");
    }

    public void Deactivate()
    {
        Debug.Log("ChargeJumping deactivated");
    }
}
