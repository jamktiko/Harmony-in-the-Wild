using UnityEngine;

public class PlayPenguinVictorySound : MonoBehaviour
{
    private AudioSource _audioSource;

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        PenguinRaceManager.instance.PenguinDungeonEvents.OnRaceFinished += PlaySoundEffect;
    }

    private void OnDisable()
    {
        PenguinRaceManager.instance.PenguinDungeonEvents.OnRaceFinished -= PlaySoundEffect;
    }

    public void PlaySoundEffect()
    {
        if (_audioSource != null)
        {
            _audioSource.Play();
        }

        else
        {
            Debug.Log(gameObject.name + " doesn't have an audio source, can't play SFX!");
        }
    }
}
