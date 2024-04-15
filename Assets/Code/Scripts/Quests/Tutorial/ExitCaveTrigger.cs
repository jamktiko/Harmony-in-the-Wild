using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitCaveTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Trigger"))
        {
            ExitCaveQuest.instance.ExitCave();
        }
    }
}
