using UnityEngine;

public class PlayPenguinVictorySound : MonoBehaviour
{
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        PenguinRaceManager.instance.penguinDungeonEvents.onRaceFinished += PlaySoundEffect;
    }

    private void OnDisable()
    {
        PenguinRaceManager.instance.penguinDungeonEvents.onRaceFinished -= PlaySoundEffect;
    }

    public void PlaySoundEffect()
    {
        if (audioSource != null)
        {
            audioSource.Play();
        }

        else
        {
            Debug.Log(gameObject.name + " doesn't have an audio source, can't play SFX!");
        }
    }
}
