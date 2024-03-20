using UnityEngine;

public class TeleGrabbing : MonoBehaviour, IAbility
{
    public void Activate()
    {
        Debug.Log("TeleGrabbing activated");
    }

    public void Deactivate()
    {
        Debug.Log("TeleGrabbing deactivated");
    }
}
