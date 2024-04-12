using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class swimmingColliders : MonoBehaviour
{
    private void Start()
    {
        if (AbilityManager.instance.abilityStatuses.TryGetValue(Abilities.Swimming, out bool isEnabled) && isEnabled)
        {
            Destroy(gameObject);
        }
    }
}
