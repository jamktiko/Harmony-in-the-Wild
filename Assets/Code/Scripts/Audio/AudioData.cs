using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AudioData
{
    public AudioName name;
    public AudioClip[] clips;
    public bool destroyAfterPlayedOnce;
    [Header("Randomized Values")]
    [Header("Pitch")]
    public bool useRandomPitch;
    public float minPitch = 0.5f;
    public float maxPitch = 1f;
    [Header("Volume")]
    public bool useRandomVolume;
    public float minVolume = 0.5f;
    public float maxVolume = 1f;
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