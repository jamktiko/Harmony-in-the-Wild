using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhatShape : MonoBehaviour
{
    [SerializeField] string ShapeName;
    [SerializeField] bool isActive;
    private void OnTriggerEnter(Collider other)
    {
        if (other.name.Contains(ShapeName)) 
        {
            isActive = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.name.Contains(ShapeName))
        {
            isActive = false;
        }
    }

}
