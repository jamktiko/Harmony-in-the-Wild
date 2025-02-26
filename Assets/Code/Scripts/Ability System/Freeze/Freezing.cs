using UnityEngine;

public class Freezing : MonoBehaviour, IAbility
{
    private void Start()
    {
        AbilityManager.Instance.RegisterAbility(Abilities.Freezing, this);
    }

    public void Activate()
    {
        Debug.Log("Freezing activated");
    }

    public void Deactivate()
    {
        Debug.Log("Freezing deactivated");
    }
}
