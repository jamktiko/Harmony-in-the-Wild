using UnityEngine;

public class Gliding : MonoBehaviour, IAbility
{
    public void Activate()
    {
        Debug.Log("Gliding activated");
    }

    public void Deactivate()
    {
        Debug.Log("Gliding deactivated");
    }
}
