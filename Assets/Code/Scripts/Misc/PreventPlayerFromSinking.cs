using UnityEngine;

public class PreventPlayerFromSinking : MonoBehaviour
{
    private Collider _ownCollider;

    private void Start()
    {
        _ownCollider = GetComponent<Collider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Trigger"))
        {
            if (other.transform.position.y < transform.position.y)
            {
                Physics.IgnoreCollision(other.GetComponent<Collider>(), _ownCollider, true);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Trigger"))
        {
            Physics.IgnoreCollision(other.GetComponent<Collider>(), _ownCollider, false);
        }
    }
}
