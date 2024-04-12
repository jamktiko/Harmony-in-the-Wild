using UnityEngine;

public class Swimming : MonoBehaviour, IAbility
{
    private void Start()
    {
        AbilityManager.instance.RegisterAbility(Abilities.Swimming, this);
    }

    public void Activate()
    {
        Debug.Log("Swimming activated");
    }

    public void Deactivate()
    {
        Debug.Log("Swimming deactivated");
    }
}
