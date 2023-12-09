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
    [SerializeField] private QuestScriptableObject questSO;

    [Tooltip("Press N & P to go to this position")]
    [SerializeField] private Transform newPosition;

    private string questId;
    private void Start()
    {
        if(questSO != null)
        {
            questId = questSO.id;
        }
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Tab)&& Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene("Overworld");
        }
        if(Input.GetKey(KeyCode.B) && Input.GetKeyDown(KeyCode.S) && questSO != null)
        {
            GameEventsManager.instance.questEvents.AdvanceDungeonQuest(questId, 1);
            SceneManager.LoadScene(bossScene);
        }

        if (Input.GetKey(KeyCode.N) && Input.GetKeyDown(KeyCode.P))
        {
            //player.GetComponent<FoxMove>().enabled = false;
            //player.GetComponent<CharacterController>().enabled = false;

            player.position = newPosition.position;

            //player.GetComponent<FoxMove>().enabled = true;
            //player.GetComponent<CharacterController>().enabled = true;
        }
    }
}
