using UnityEngine;

public class GhostSpeaking : MonoBehaviour, IAbility
{
    public void Activate()
    {
        Debug.Log("GhostSpeaking activated");
    }

    public void Deactivate()
    {
        Debug.Log("GhostSpeaking deactivated");
    }
}
