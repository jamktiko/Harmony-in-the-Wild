using UnityEngine;

public class SnowDiving : MonoBehaviour, IAbility
{
    private void Start()
    {
        AbilityManager.instance.RegisterAbility(Abilities.SnowDiving, this);
    }

    public void Activate()
    {
        Debug.Log("SnowDiving activated");
    }

    public void Deactivate()
    {
        Debug.Log("SnowDiving deactivated");
    }
}
