using UnityEngine;

public class PlaySound : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;

    private void OnTriggerEnter(Collider other)
    {
        audioSource.Play();
    }
    private void OnTriggerExit(Collider other)
    {
        audioSource.Stop();
    }
}
