using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitCounter : MonoBehaviour
{
    [Header("Config")]
    [Tooltip("Maximum amount of hits before the player is transformed to the starting spot")]
    [SerializeField] private int maxHits;

    [Header("Needed References")]
    [SerializeField] private Transform startingSpot;

    private int currentHits;
    
    public void TakeHit()
    {
        Debug.Log("Player was hit by projectile!");

        currentHits++;

        if(currentHits >= maxHits)
        {
            // move player back to starting spot
            GetComponent<FoxMove>().enabled = false;
            GetComponent<CharacterController>().enabled = false;

            transform.position = startingSpot.position;

            GetComponent<FoxMove>().enabled = true;
            GetComponent<CharacterController>().enabled = true;

            // reset current hits
            currentHits = 0;
        }
    }
}