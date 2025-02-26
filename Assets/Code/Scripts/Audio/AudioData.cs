using UnityEngine;
using UnityEngine.Serialization;

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
    [FormerlySerializedAs("_useRandomVolume")] [Header("Volume")]
    public bool UseRandomVolume;
    public float MinVolume = 0.5f;
    public float MaxVolume = 1f;
}

[System.Serializable]
public class AudioObject
{
    [FormerlySerializedAs("name")] public AudioName Name;
    [FormerlySerializedAs("audioPrefab")] public GameObject AudioPrefab;
    [FormerlySerializedAs("forceToBeUnderPlayer")] [Tooltip("Tick this if the audio needs to spawn at the player's position, but it doesn't have the reference for it (called from Ability Manager etc.).")]
    public bool ForceToBeUnderPlayer;
}

[System.Serializable]
public class ThemeData
{
    [FormerlySerializedAs("name")] public ThemeName Name;
    [FormerlySerializedAs("clip")] public AudioClip Clip;
    [FormerlySerializedAs("maxVolume")] public float MaxVolume;
}