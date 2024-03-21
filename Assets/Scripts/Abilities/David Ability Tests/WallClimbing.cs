using UnityEngine;

public class WallClimbing : MonoBehaviour, IAbility
{
    public void Activate()
    {
        Debug.Log("WallClimbing activated");
    }

    public void Deactivate()
    {
        Debug.Log("WallClimbing deactivated");
    }
}
