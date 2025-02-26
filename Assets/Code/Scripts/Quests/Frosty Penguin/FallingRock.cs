using System.Collections;
using UnityEngine;

public class FallingRock : MonoBehaviour
{
    [SerializeField] private GameObject destroyEffect;
    [SerializeField] private float selfDestructionTime;

    private void OnTriggerEnter(Collider other)
    {
        AudioManager.Instance.PlaySound(AudioName.Prop_RockFalling, transform);

        if (other.gameObject.CompareTag("Trigger"))
        {
            Instantiate(destroyEffect, transform.position, Quaternion.identity);

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
