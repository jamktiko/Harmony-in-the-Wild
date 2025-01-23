using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPrefabObject : MonoBehaviour
{
    [SerializeField] private AudioData data;

    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();

        if(audioSource == null)
        {
            Debug.Log($"No audio source found for {gameObject.name}!");
        }

        PlayAudio();

        if (!data.destroyAfterPlayedOnce)
        {
            GameEventsManager.instance.audioEvents.OnDestroyAudio += DestroyOnCall;
        }
    }

    private void OnDisable()
    {
        if (!data.destroyAfterPlayedOnce)
        {
            GameEventsManager.instance.audioEvents.OnDestroyAudio -= DestroyOnCall;
        }
    }

    private void PlayAudio()
    {
        if(data.clips.Length <= 0)
        {
            Debug.Log($"No audio clips assigned for {gameObject.name}.");
            return;
        }

        audioSource.clip = data.clips[Random.Range(0, data.clips.Length)];

        if (data.useRandomPitch)
        {
            audioSource.pitch = Random.Range(data.minPitch, data.maxPitch);
        }

        if (data.useRandomVolume)
        {
            audioSource.volume = Random.Range(data.minVolume, data.maxVolume);
        }

        audioSource.Play();

        if (data.destroyAfterPlayedOnce)
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
        if(audioToDestroy == data.name)
        {
            Destroy(gameObject);
        }
    }
}
