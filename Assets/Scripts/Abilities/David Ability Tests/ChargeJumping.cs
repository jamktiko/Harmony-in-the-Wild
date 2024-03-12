using UnityEngine;

public class ChargeJumping : MonoBehaviour, IAbility
{
    public void Activate()
    {
        Debug.Log("ChargeJumping activated");
    }

    public void Deactivate()
    {
        Debug.Log("ChargeJumping deactivated");
    }
}
