using UnityEngine;

public class TeleGrabbing : MonoBehaviour, IAbility
{
    private void Start()
    {
        AbilityManager.instance.RegisterAbility(Abilities.TeleGrabbing, this);
    }

    public void Activate()
    {
        Debug.Log("TeleGrabbing activated");
    }

    public void Deactivate()
    {
        Debug.Log("TeleGrabbing deactivated");
    }
}
