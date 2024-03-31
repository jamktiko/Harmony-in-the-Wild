using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ore : MonoBehaviour
{
    private bool playerIsNear;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && playerIsNear)
        {

            Invoke("DestroyObject", 1);
            SmashingReturnOre.instance.PickUpOre();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerIsNear = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerIsNear = false;
        }
    }

    private void DestroyObject()
    {
        if (gameObject != null)
        {
            Destroy(gameObject);
        }
    }
}
