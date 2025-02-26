using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

public class FallingRock : MonoBehaviour
{
    [FormerlySerializedAs("destroyEffect")] [SerializeField] private GameObject _destroyEffect;
    [FormerlySerializedAs("selfDestructionTime")] [SerializeField] private float _selfDestructionTime;

    private void OnTriggerEnter(Collider other)
    {
        AudioManager.Instance.PlaySound(AudioName.PropRockFalling, transform);

        if (other.gameObject.CompareTag("Trigger"))
        {
            Instantiate(_destroyEffect, transform.position, Quaternion.identity);

            other.GetComponentInParent<HitCounter>().TakeHit(false);
        }

        else
        {
            StartCoroutine(SelfDestruction());
        }
    }

    private IEnumerator SelfDestruction()
    {
        yield return new WaitForSeconds(_selfDestructionTime);

        Instantiate(_destroyEffect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
