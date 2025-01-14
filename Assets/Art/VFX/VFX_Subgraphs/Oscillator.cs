using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class Oscillator : MonoBehaviour
{
    public VisualEffect vfx;
    // step count in one direction from 0
    public float frequency = 100;
    private float step;
    private float curValue;
    private float curSpeed;

    void Awake()
    {
        step = 2;
        curSpeed = 2;
        curValue = -1;
    }

    // Update is called once per frame
    void Update()
    {
        if (curValue > 0) curSpeed -= step * Time.deltaTime * frequency;
        else curSpeed += step * Time.deltaTime * frequency;
        curValue += curSpeed * Time.deltaTime;
        if (vfx)
            vfx.SetFloat("Oscillation", curValue);
    }
}
