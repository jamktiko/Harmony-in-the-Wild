using UnityEngine;
using UnityEngine.Serialization;

public class PlaySound : MonoBehaviour
{
    [FormerlySerializedAs("audioSource")] [SerializeField] AudioSource _audioSource;

    private void OnTriggerEnter(Collider other)
    {
        _audioSource.Play();
    }
    private void OnTriggerExit(Collider other)
    {
        _audioSource.Stop();
    }
}
