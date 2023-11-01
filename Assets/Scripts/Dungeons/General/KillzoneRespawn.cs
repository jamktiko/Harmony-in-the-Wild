using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillzoneRespawn : MonoBehaviour
{
    [SerializeField] private Transform respawnPosition;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            //other.GetComponent<FoxMove>().enabled = false;
            //other.GetComponent<CharacterController>().enabled = false;

            other.transform.position = respawnPosition.position;

            //other.GetComponent<FoxMove>().enabled = true;
            //other.GetComponent<CharacterController>().enabled = true;
        }
    }
}
