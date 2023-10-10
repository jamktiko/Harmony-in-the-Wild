using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTeleportation : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private Transform teleportationTarget;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("enter");
        if (other.CompareTag("Player"))
        {
            Debug.Log("player entered");
            player.position = teleportationTarget.position;
        }
    }
}
