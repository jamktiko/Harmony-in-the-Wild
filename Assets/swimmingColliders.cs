using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class swimmingColliders : MonoBehaviour
{
    private void Start()
    {
        if (PlayerManager.instance.abilityValues[1] == true) 
        {
            Destroy(gameObject);
        }
    }
}
