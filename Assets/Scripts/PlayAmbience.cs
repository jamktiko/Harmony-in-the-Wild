using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAmbience : MonoBehaviour
{
    [SerializeField] AudioSource forest;
    [SerializeField] AudioSource arctic;
    [SerializeField] AudioSource arcticTheme;
    [SerializeField] AudioSource forestTheme;
    [SerializeField] PlayerModelToggle toggle;
    private void OnTriggerEnter(Collider other)
    {
        toggle.TogglePlayerModel();
        if (forest.isPlaying)
        {
            //Forest.Stop();
            forestTheme.Stop();
            //Arctic.Play();
            arcticTheme.Play();
        }
        else if (arctic.isPlaying)
        {
            //Arctic.Stop();
            arcticTheme.Stop();
            //Forest.Play();
            forestTheme.Play();
        }
    }
}
