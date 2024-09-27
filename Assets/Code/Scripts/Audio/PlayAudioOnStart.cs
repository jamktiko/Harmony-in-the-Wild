using UnityEngine;

public class PlayAudioOnStart : MonoBehaviour
{
    void Start()
    {
        GetComponentInChildren<AudioSource>().Play();
    }
}
