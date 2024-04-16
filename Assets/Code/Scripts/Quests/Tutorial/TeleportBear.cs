using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportBear : MonoBehaviour
{
    [SerializeField] private Vector3 spawnPosition;

    private void Start()
    {
        Transform bear = transform.parent.Find("TutorialBear");

        bear.position = spawnPosition;
        bear.localRotation = Quaternion.Euler(0, -66, 0);
    }
}