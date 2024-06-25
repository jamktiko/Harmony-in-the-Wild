using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportUsingMap : MonoBehaviour
{
    private Transform player;
    private void Start()
    {
        GameObject[] playerTags = GameObject.FindGameObjectsWithTag("Player");

        foreach (GameObject playerOption in playerTags)
        {
            if (playerOption.gameObject.name == "Player_Spawn")
            {
                player = playerOption.transform;
            }
        }
    }
    public void TeleportTo()
    {
        //Debug.Log("Hello yes I been clicked");

        player.gameObject.SetActive(false);
        player.transform.position = transform.position;
        player.gameObject.SetActive(true);
    }
}
