using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTeleportation : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private Transform teleportationTarget;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            player.GetComponent<FoxMove>().enabled = false;
            player.GetComponent<CharacterController>().enabled = false;
            player.position = teleportationTarget.position;
            player.GetComponent<FoxMove>().enabled = true;
            player.GetComponent<CharacterController>().enabled = true;
        }
    }
}
