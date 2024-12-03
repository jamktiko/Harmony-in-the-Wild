using System.Collections;
using UnityEngine;

public class FallingRock : MonoBehaviour
{
    [SerializeField] private GameObject destroyEffect;
    [SerializeField] private float selfDestructionTime;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Trigger"))
        {
            GetComponent<RandomizeAudioValues>().PlaySound();
            Instantiate(destroyEffect, transform.position, Quaternion.identity);
            Destroy(gameObject, 1f); // NOTE! delete this when full audio revamp has been done

            other.GetComponentInParent<HitCounter>().TakeHit(false);
        }

        else
        {
            GetComponent<RandomizeAudioValues>().PlaySound();
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
