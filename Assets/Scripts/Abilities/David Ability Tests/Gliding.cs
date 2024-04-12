using UnityEngine;

public class Gliding : MonoBehaviour, IAbility
{
    public void Start()
    {
        AbilityManager.instance.RegisterAbility(Abilities.Gliding, this);
    }

    public void Activate()
    {
        Debug.Log("Gliding activated");
    }

    public void Deactivate()
    {
        Debug.Log("Gliding deactivated");
    }
}
