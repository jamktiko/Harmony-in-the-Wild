using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DevShortCuts : MonoBehaviour
{
    [Header("Needed References")]
    [SerializeField] private Transform player;

    [Header("Shortcut Config")]
    [Tooltip("Press B & S to go to this boss scene")]
    [SerializeField] private string bossScene;

    [Tooltip("Press N & P to go to this position")]
    [SerializeField] private Transform newPosition;

    void Update()
    {
        if(Input.GetKey(KeyCode.B) && Input.GetKeyDown(KeyCode.S))
        {
            SceneManager.LoadScene(bossScene);
        }

        if (Input.GetKey(KeyCode.N) && Input.GetKeyDown(KeyCode.P))
        {
            player.GetComponent<FoxMove>().enabled = false;
            player.GetComponent<CharacterController>().enabled = false;

            player.position = newPosition.position;

            player.GetComponent<FoxMove>().enabled = true;
            player.GetComponent<CharacterController>().enabled = true;
        }
    }
}
