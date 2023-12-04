using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DungeonTransition : MonoBehaviour
{
    [Header("Dungeon Quest Config")]
    [SerializeField] private QuestScriptableObject questSO;

    [Header("Config")]
    [SerializeField] private string goToScene;
    
    private string questId;

    private void Start()
    {
        questId = questSO.id;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            GameEventsManager.instance.questEvents.AdvanceDungeonQuest(questId);

            SceneManager.LoadScene(goToScene);
        }
    }

}
