using UnityEngine;

public class RandomizeAudioValues : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] private float minPitchValue = 0.5f;
    [SerializeField] private float maxPitchValue = 1f;

    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlaySound()
    {
        audioSource.pitch = Random.Range(minPitchValue, maxPitchValue);
        audioSource.Play();
    }
}
