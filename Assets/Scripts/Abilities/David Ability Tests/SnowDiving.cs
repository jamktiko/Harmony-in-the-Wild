using UnityEngine;

public class SnowDiving : MonoBehaviour, IAbility
{
    public void Activate()
    {
        Debug.Log("SnowDiving activated");
    }

    public void Deactivate()
    {
        Debug.Log("SnowDiving deactivated");
    }
}
