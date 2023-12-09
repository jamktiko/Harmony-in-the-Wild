using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playsound : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;

    private void OnTriggerEnter(Collider other)
    {
        audioSource.Play();
    }
    private void OnTriggerExit(Collider other)
    {
        audioSource.Stop();
    }
}
