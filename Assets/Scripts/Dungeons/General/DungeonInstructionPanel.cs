using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonInstructionPanel : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            gameObject.SetActive(false);
        }
    }
}
