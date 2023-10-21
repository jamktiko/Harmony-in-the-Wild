using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingRock : MonoBehaviour
{
    [SerializeField] private GameObject destroyEffect;
    [SerializeField] private float selfDestructionTime;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Instantiate(destroyEffect, transform.position, Quaternion.identity);
            Destroy(gameObject);

            // player should be moved to starting position
        }

        else
        {
            StartCoroutine(SelfDestruction());
        }
    }

    private IEnumerator SelfDestruction()
    {
        yield return new WaitForSeconds(selfDestructionTime);

        Instantiate(destroyEffect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
