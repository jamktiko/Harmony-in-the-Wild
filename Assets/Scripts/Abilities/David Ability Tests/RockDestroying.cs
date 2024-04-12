using UnityEngine;

public class RockDestroying : MonoBehaviour, IAbility
{
    private void Start()
    {
        AbilityManager.instance.RegisterAbility(Abilities.RockDestroying, this);
    }

    public void Activate()
    {
        Debug.Log("RockDestroying activated");
    }

    public void Deactivate()
    {
        Debug.Log("RockDestroying deactivated");
    }
}
