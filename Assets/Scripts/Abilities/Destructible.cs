using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructible : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] private bool needsToBeFreezed;

    [Header("Needed References")]
    [SerializeField] private GameObject destroyedVersion;

    private void Update()
    {
        // NOTE DEBUGGING ONLY FOR NOW
        if (Input.GetKey(KeyCode.R) && Input.GetKeyDown(KeyCode.S))
        {
            PlayerManager.instance.abilityValues[4] = true;
            Debug.Log("Player can rock smash now.");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && PlayerManager.instance.abilityValues[4])
        {
            if (needsToBeFreezed)
            {
                if (gameObject.GetComponent<Freezable>().isFreezed)
                {
                    Debug.Log("Using rock smash on " + gameObject.name);
                    Instantiate(destroyedVersion, transform.position, Quaternion.identity);
                    Destroy(gameObject);
                }

                else
                {
                    Debug.Log(gameObject.name + " needs to be freezed first.");
                }
            }

            else
            {
                Instantiate(destroyedVersion, transform.position, Quaternion.identity);
                Destroy(gameObject);
            }
        }
    }
}
