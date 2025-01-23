using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioData
{
    public AudioName name;
    public AudioClip clip;
    public bool destroyOnInput;
    public bool useRandomPitch;
    public float minPitch = 0.5f;
    public float maxPitch = 1f;
}

public class AudioObject
{
    public AudioName name;
    public GameObject audioPrefab;
}