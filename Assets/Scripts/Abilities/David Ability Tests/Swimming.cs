using UnityEngine;

public class Swimming : MonoBehaviour, IAbility
{
    public void Activate()
    {
        Debug.Log("Swimming activated");
    }

    public void Deactivate()
    {
        Debug.Log("Swimming deactivated");
    }
}
