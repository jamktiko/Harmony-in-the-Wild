using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TeleportButton : MonoBehaviour
{
    private Vector3 teleportTarget;
    private Transform player;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    public void InitializeLocation(Vector3 newTarget, string name)
    {
        teleportTarget = newTarget;
        transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = name;
    }

    public void Teleport()
    {
        player.GetComponent<FoxMovement>().enabled = false;
        player.position = teleportTarget;
        player.GetComponent<FoxMovement>().enabled = true;
    }
}
