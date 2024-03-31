using UnityEngine;

public class WhatShape : MonoBehaviour
{
    [SerializeField] string shapeName;
    [SerializeField] bool isActive; //NOTE: What does this bool check?
    [SerializeField] AudioSource correctAudio;

    private void OnTriggerEnter(Collider other)
    {
        if (other.name.Contains(shapeName)) 
        {
            isActive = true;
            correctAudio.Play();

            //BossDoorMonkey.instance.UpdateProgress(1);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.name.Contains(shapeName))
        {
            isActive = false;

            //BossDoorMonkey.instance.UpdateProgress(-1);
        }
    }
}
