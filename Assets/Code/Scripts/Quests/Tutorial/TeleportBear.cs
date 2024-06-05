using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportBear : MonoBehaviour
{
    [SerializeField] private Vector3 spawnPosition;

    private Transform bear;

    private void Start()
    {
        try
        {
            bear = transform.parent.Find("TutorialBear(Clone)");
            Debug.Log("Target position: " + spawnPosition);
            bear.position = spawnPosition;
            bear.localRotation = Quaternion.Euler(0, -66, 0);
            Debug.Log("Teleported to: " + bear.position);
        } 
        catch 
        {
            
        }
        

        
    }

    //private void Update()
    //{
    //    bear.position = spawnPosition;
    //}
}