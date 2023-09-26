using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructible : MonoBehaviour
{
    [SerializeField] private GameObject destroyedVersion;

    private void Update()
    {
        // NOTE DEBUGGING ONLY FOR NOW
        if (Input.GetKey(KeyCode.R) && Input.GetKey(KeyCode.S))
        {
            PlayerManager.instance.abilityValues[4] = true;
            Debug.Log("Player can rock smash now.");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && PlayerManager.instance.abilityValues[4])
        {
            Instantiate(destroyedVersion, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
