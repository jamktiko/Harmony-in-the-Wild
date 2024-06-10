using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportBear : MonoBehaviour
{
    [SerializeField] private Vector3 spawnPosition;
    private Vector3 rotation = new Vector3(0, -66, 0);

    private Transform bear;

    private void Start()
    {
        bear = transform.parent.Find("TutorialBear(Clone)");

        if(bear != null)
        {
            bear.position = spawnPosition;
            bear.localRotation = Quaternion.Euler(rotation);
        }
    }
}