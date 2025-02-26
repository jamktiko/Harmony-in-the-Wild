using UnityEngine;
using UnityEngine.Serialization;

public class RandomizeAudioValues : MonoBehaviour
{
    [FormerlySerializedAs("minPitchValue")]
    [Header("Config")]
    [SerializeField] private float _minPitchValue = 0.5f;
    [FormerlySerializedAs("maxPitchValue")] [SerializeField] private float _maxPitchValue = 1f;

    private AudioSource _audioSource;

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    public void PlaySound()
    {
        _audioSource.pitch = Random.Range(_minPitchValue, _maxPitchValue);
        _audioSource.Play();
    }
}
