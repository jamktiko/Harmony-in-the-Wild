using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerTeleportation : MonoBehaviour
{
    [FormerlySerializedAs("player")] [SerializeField] private Transform _player;
    [FormerlySerializedAs("teleportationTarget")] [SerializeField] private Transform _teleportationTarget;

    private AudioSource _audioSource;

    private void Start()
    {
        //audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Trigger"))
        {
            StartCoroutine(Teleport());
        }
    }

    private IEnumerator Teleport()
    {
        //audioSource.Play();

        //yield return new WaitForSeconds(audioSource.clip.length * 0.75f);
        yield return new WaitForSeconds(0.2f);
        FoxMovement.Instance.gameObject.SetActive(false);
        _player.position = _teleportationTarget.position;
        FoxMovement.Instance.gameObject.SetActive(true);
        Debug.Log("teleporting player");
    }
}
