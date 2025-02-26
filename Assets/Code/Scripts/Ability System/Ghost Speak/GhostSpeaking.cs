using UnityEngine;

public class GhostSpeaking : MonoBehaviour, IAbility
{
    private void Start()
    {
        AbilityManager.Instance.RegisterAbility(Abilities.GhostSpeaking, this);
    }

    public void Activate()
    {
        Debug.Log("GhostSpeaking activated");
    }

    public void Deactivate()
    {
        Debug.Log("GhostSpeaking deactivated");
    }
}
