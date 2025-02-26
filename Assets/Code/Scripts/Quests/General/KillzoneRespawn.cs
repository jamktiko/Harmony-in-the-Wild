using UnityEngine;
using UnityEngine.Serialization;

public class KillzoneRespawn : MonoBehaviour
{
    [FormerlySerializedAs("respawnPosition")] [SerializeField] private Transform _respawnPosition;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Trigger"))
        {

            //other.GetComponent<FoxMove>().enabled = false;
            //other.GetComponent<CharacterController>().enabled = false;

            other.transform.parent.position = _respawnPosition.position;

            //other.GetComponent<FoxMove>().enabled = true;
            //other.GetComponent<CharacterController>().enabled = true;
        }
    }
}
