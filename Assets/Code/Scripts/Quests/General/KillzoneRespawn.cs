using UnityEngine;

public class KillzoneRespawn : MonoBehaviour
{
    [SerializeField] private Transform respawnPosition;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Trigger"))
        {

            //other.GetComponent<FoxMove>().enabled = false;
            //other.GetComponent<CharacterController>().enabled = false;

            other.transform.parent.position = respawnPosition.position;

            //other.GetComponent<FoxMove>().enabled = true;
            //other.GetComponent<CharacterController>().enabled = true;
        }
    }
}
