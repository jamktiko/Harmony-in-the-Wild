using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostSpeak : MonoBehaviour
{
    private void Update()
    {
        // NOTE DEBUGGING ONLY FOR NOW
        // NOTE THIS SCRIPT CAN BE DELETED COMPLETELY ONCE THE QUEST UNLOCKING GHOST SPEAK IS IMPLEMENTED
        if (Input.GetKey(KeyCode.G) && Input.GetKeyDown(KeyCode.S))
        {
            PlayerManager.instance.getAbility(5);
            Debug.Log("Player can use ghost speak now.");
        }
    }
}
