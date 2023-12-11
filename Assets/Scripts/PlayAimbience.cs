using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAimbience : MonoBehaviour
{
    [SerializeField] AudioSource Forest;
    [SerializeField] AudioSource Arctic;
    [SerializeField] AudioSource arcticTheme;
    [SerializeField] AudioSource forestTheme;

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (Forest.isPlaying)
        {
            Forest.Stop();
            forestTheme.Stop();
            Arctic.Play();
            arcticTheme.Play();
        }
        else if (Arctic.isPlaying)
        {
            Arctic.Stop();
            arcticTheme.Stop();
            Forest.Play();
            forestTheme.Play();
        }
    }
}
