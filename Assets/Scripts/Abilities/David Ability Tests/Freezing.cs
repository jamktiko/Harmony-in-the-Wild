using UnityEngine;

public class Freezing : MonoBehaviour, IAbility
{
    public void Activate()
    {
        Debug.Log("Freezing activated");
    }

    public void Deactivate()
    {
        Debug.Log("Freezing deactivated");
    }
}
