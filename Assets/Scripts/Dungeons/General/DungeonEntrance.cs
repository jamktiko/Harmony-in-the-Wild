using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(BoxCollider))]
public class DungeonEntrance : MonoBehaviour
{
    [Header("Quest")]
    [SerializeField] private QuestScriptableObject dungeonQuest;

    [Header("Needed References")]
    [SerializeField] private GameObject dungeonEnteringPrecentedUI;

    [Header("Config")]
    [SerializeField] private string goToScene;

    private string questId;
    private QuestState currentQuestState;
    private Quest currentQuest;

    private void Start()
    {
        questId = dungeonQuest.id;
        StartCoroutine(QuestProgressCheckDelay());
    }

    private void OnEnable()
    {
        GameEventsManager.instance.questEvents.onQuestStateChange += QuestStateChange;
    }

    private void OnDisable()
    {
        GameEventsManager.instance.questEvents.onQuestStateChange -= QuestStateChange;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if(currentQuestState == QuestState.CAN_START)
            {
                GameEventsManager.instance.questEvents.StartQuest(questId);
                SceneManager.LoadScene(goToScene);
            }

            else if (currentQuestState == QuestState.IN_PROGRESS)
            {
                SceneManager.LoadScene(goToScene);
            }

            else
            {
                dungeonEnteringPrecentedUI.SetActive(true);
                dungeonEnteringPrecentedUI.GetComponent<DungeonEnteringPreventedUI>().SetUIContent(currentQuest);
            }
        }
    }

    private void QuestStateChange(Quest quest)
    {
        // only update the quest state if this point has the corresponding quest
        if (quest.info.id.Equals(questId))
        {
            currentQuestState = quest.state;
            Debug.Log("Quest with id: " + questId + " updated to state: " + currentQuestState);
        }
    }

    // NOTE! CHANGE THIS TO START ONCE THE MANAGERS ARE INITIALIZED IN MAIN MENU
    // NOTE! THIS DELAY IS HERE FOR TESTING PURPOSES ONLY TO MAKE SURE THE SAVED DATA IS LOADED BEFORE TRYING TO REFERENCE TO IT
    private IEnumerator QuestProgressCheckDelay()
    {
        yield return new WaitForSeconds(1);

        currentQuest = QuestManager.instance.GetQuestById(questId);

        currentQuestState = currentQuest.state;
    }
}
