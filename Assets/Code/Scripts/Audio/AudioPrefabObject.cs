using UnityEngine;
using UnityEngine.Serialization;

public class AudioPrefabObject : MonoBehaviour
{
    [FormerlySerializedAs("data")] [SerializeField] private AudioData _data;

    private AudioSource _audioSource;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();

        if (_audioSource == null)
        {
            Debug.Log($"No audio source found for {gameObject.name}!");
        }

        PlayAudio();

        if (!_data.DestroyAfterPlayedOnce)
        {
            GameEventsManager.instance.AudioEvents.OnDestroyAudio += DestroyOnCall;
        }

        GameEventsManager.instance.PlayerEvents.OnToggleInputActions += ToggleAudioOnInput;
    }

    private void OnDisable()
    {
        if (!_data.DestroyAfterPlayedOnce)
        {
            GameEventsManager.instance.AudioEvents.OnDestroyAudio -= DestroyOnCall;
        }

        GameEventsManager.instance.PlayerEvents.OnToggleInputActions -= ToggleAudioOnInput;
    }

    private void PlayAudio()
    {
        if (_data.ClipsList.Length <= 0)
        {
            Debug.Log($"No audio clips assigned for {gameObject.name}.");
            return;
        }

        _audioSource.clip = _data.ClipsList[Random.Range(0, _data.ClipsList.Length)];

        if (_data.UseRandomPitch)
        {
            _audioSource.pitch = Random.Range(_data.MinPitch, _data.MaxPitch);
        }

        if (_data.UseRandomVolume)
        {
            _audioSource.volume = Random.Range(_data.MinVolume, _data.MaxVolume);
        }

        _audioSource.Play();

        if (_data.DestroyAfterPlayedOnce)
        {
            Invoke(nameof(DestroyAfterClipPlayedOnce), _audioSource.clip.length);
        }
    }

    private void DestroyAfterClipPlayedOnce()
    {
        Destroy(gameObject);
    }

    private void DestroyOnCall(AudioName audioToDestroy)
    {
        if (audioToDestroy == _data.Name)
        {
            Destroy(gameObject);
        }
    }

    private void ToggleAudioOnInput(bool audioOn)
    {
        if (audioOn)
        {
            _audioSource.Play();
        }

        else
        {
            _audioSource.Pause();
        }
    }
}
