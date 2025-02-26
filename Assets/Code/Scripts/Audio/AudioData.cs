using UnityEngine;

[System.Serializable]
public class AudioData
{
    public AudioName Name;
    public AudioClip[] ClipsList;
    public bool DestroyAfterPlayedOnce;
    [Header("Randomized Values")]
    [Header("Pitch")]
    public bool UseRandomPitch;
    public float MinPitch = 0.5f;
    public float MaxPitch = 1f;
    [Header("Volume")]
    public bool _useRandomVolume;
    public float MinVolume = 0.5f;
    public float MaxVolume = 1f;
}

[System.Serializable]
public class AudioObject
{
    public AudioName name;
    public GameObject audioPrefab;
    [Tooltip("Tick this if the audio needs to spawn at the player's position, but it doesn't have the reference for it (called from Ability Manager etc.).")]
    public bool forceToBeUnderPlayer;
}

[System.Serializable]
public class ThemeData
{
    public ThemeName name;
    public AudioClip clip;
    public float maxVolume;
}