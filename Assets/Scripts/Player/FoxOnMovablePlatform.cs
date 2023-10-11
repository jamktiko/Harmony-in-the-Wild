using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoxOnMovablePlatform : MonoBehaviour
{
    private CharacterController controller;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    private void LateUpdate()
    {
        //controller.M
    }
}
