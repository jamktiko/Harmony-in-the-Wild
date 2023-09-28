using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Freeze : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] private float aoeRadius;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            ActivateFreeze();
        }
    }

    private void ActivateFreeze()
    {
        Collider[] foundObjects = Physics.OverlapSphere(transform.position, aoeRadius, LayerMask.GetMask("Freezables"));

        if(foundObjects != null)
        {
            foreach(Collider newObject in foundObjects)
            {
                Freezable freezable = newObject.gameObject.GetComponent<Freezable>();

                if (freezable)
                {
                    freezable.Freeze();
                    return;
                }
            }
        }

        Debug.Log("No colliders detected for freezing.");
    }
}
