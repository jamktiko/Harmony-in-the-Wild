using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreventPlayerFromSinking : MonoBehaviour
{
    private Collider ownCollider;

    private void Start()
    {
        ownCollider = GetComponent<Collider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Trigger"))
        {
            if(other.transform.position.y < transform.position.y)
            {
                Physics.IgnoreCollision(other.GetComponent<Collider>(), ownCollider, true);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Trigger"))
        {
            Physics.IgnoreCollision(other.GetComponent<Collider>(), ownCollider, false);
        }
    }
}
