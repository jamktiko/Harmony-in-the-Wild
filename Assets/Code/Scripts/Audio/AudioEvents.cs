using System;

public class AudioEvents
{
    public event Action<AudioName> OnDestroyAudio;

    public void DestroyAudio(AudioName audioToDestroy)
    {
        OnDestroyAudio?.Invoke(audioToDestroy);
    }
}
