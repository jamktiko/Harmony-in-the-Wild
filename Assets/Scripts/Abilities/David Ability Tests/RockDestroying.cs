using UnityEngine;

public class RockDestroying : MonoBehaviour, IAbility
{
    public void Activate()
    {
        Debug.Log("RockDestroying activated");
    }

    public void Deactivate()
    {
        Debug.Log("RockDestroying deactivated");
    }
}
