using UnityEngine;

public class PlayAudioOnStart : MonoBehaviour
{
    private void Start()
    {
        GetComponentInChildren<AudioSource>().Play();
    }
}
