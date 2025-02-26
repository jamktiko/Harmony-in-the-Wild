using UnityEngine;

public class AudioPrefabObject : MonoBehaviour
{
    [SerializeField] private AudioData data;

    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();

        if (audioSource == null)
        {
            Debug.Log($"No audio source found for {gameObject.name}!");
        }

        PlayAudio();

        if (!data.DestroyAfterPlayedOnce)
        {
            GameEventsManager.instance.audioEvents.OnDestroyAudio += DestroyOnCall;
        }

        GameEventsManager.instance.playerEvents.OnToggleInputActions += ToggleAudioOnInput;
    }

    private void OnDisable()
    {
        if (!data.DestroyAfterPlayedOnce)
        {
            GameEventsManager.instance.audioEvents.OnDestroyAudio -= DestroyOnCall;
        }

        GameEventsManager.instance.playerEvents.OnToggleInputActions -= ToggleAudioOnInput;
    }

    private void PlayAudio()
    {
        if (data.ClipsList.Length <= 0)
        {
            Debug.Log($"No audio clips assigned for {gameObject.name}.");
            return;
        }

        audioSource.clip = data.ClipsList[Random.Range(0, data.ClipsList.Length)];

        if (data.UseRandomPitch)
        {
            audioSource.pitch = Random.Range(data.MinPitch, data.MaxPitch);
        }

        if (data._useRandomVolume)
        {
            audioSource.volume = Random.Range(data.MinVolume, data.MaxVolume);
        }

        audioSource.Play();

        if (data.DestroyAfterPlayedOnce)
        {
            Invoke(nameof(DestroyAfterClipPlayedOnce), audioSource.clip.length);
        }
    }

    private void DestroyAfterClipPlayedOnce()
    {
        Destroy(gameObject);
    }

    private void DestroyOnCall(AudioName audioToDestroy)
    {
        if (audioToDestroy == data.Name)
        {
            Destroy(gameObject);
        }
    }

    private void ToggleAudioOnInput(bool audioOn)
    {
        if (audioOn)
        {
            audioSource.Play();
        }

        else
        {
            audioSource.Pause();
        }
    }
}
