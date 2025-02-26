using UnityEngine;

public class ZoneTransitionEnabler : MonoBehaviour
{
    void Start()
    {
        Invoke(nameof(EnableZoneTriggers), 0.4f);
    }

    private void EnableZoneTriggers()
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(true);
        }
    }
}
