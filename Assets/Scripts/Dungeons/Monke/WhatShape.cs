using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhatShape : MonoBehaviour
{
    [SerializeField] string ShapeName;
    [SerializeField] bool isActive;
    [SerializeField] AudioSource correctAudio;
    private void OnTriggerEnter(Collider other)
    {
        if (other.name.Contains(ShapeName)) 
        {
            isActive = true;
            correctAudio.Play();

            BossDoorMonkey.instance.UpdateProgress(1);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.name.Contains(ShapeName))
        {
            isActive = false;

            BossDoorMonkey.instance.UpdateProgress(-1);
        }
    }

}
