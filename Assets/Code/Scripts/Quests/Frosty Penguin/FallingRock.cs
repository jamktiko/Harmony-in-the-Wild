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
            Instantiate(destroyEffect, transform.position, Quaternion.identity);
            Destroy(gameObject);

            other.GetComponentInParent<HitCounter>().TakeHit(false);
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
