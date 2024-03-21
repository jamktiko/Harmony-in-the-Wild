using UnityEngine;

public class TestAbility : MonoBehaviour, IAbility
{
    public void Activate()
    {
        Debug.Log("Test Ability activated");
    }

    public void Deactivate()
    {
        Debug.Log("Test Ability deactivated");
    }
}
