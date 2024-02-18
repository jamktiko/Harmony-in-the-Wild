using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(BoxCollider))]
public class DungeonEntrance : MonoBehaviour
{
    [Header("Quest")]
    [SerializeField] private QuestScriptableObject dungeonQuest;

    [Header("Needed References")]
    [SerializeField] private GameObject dungeonEnteringPreventedUI;

    [Header("Config")]
    [SerializeField] private string goToScene; //NOTE: Doesn't feel like the best way to handle scene changes
    [SerializeField] private int storybookSectionIndex;
    [Tooltip("Tick if a quest is started when entering this dungeon")]
    [SerializeField] private bool activateQuestProgressTracking;

    private string questId;
    private QuestState currentQuestState;
    private Quest currentQuest;
    [SerializeField]private GameObject loadingScreen;
    [SerializeField]private TMP_Text loadingScreenText;

    private void Start()
    {
        if(dungeonQuest != null)
        {
            questId = dungeonQuest.id;
            StartCoroutine(QuestProgressCheckDelay());
        }
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
        if (other.gameObject.CompareTag("Trigger"))
        {
            if(currentQuestState == QuestState.CAN_START)
            {
                if (activateQuestProgressTracking)
                {
                    GameEventsManager.instance.questEvents.StartQuest(questId);
                }

                // add storybook config here & change goToScene to Storybook scene
                StorybookHandler.instance.SetNewStorybookData(storybookSectionIndex, goToScene, false);
                StartCoroutine(loadSceneWithLoadingScreenWithText(2));
            }

            else if (currentQuestState == QuestState.IN_PROGRESS)
            {
                // add possible storybook config here & change goToScene to Storybook scene
                StorybookHandler.instance.SetNewStorybookData(storybookSectionIndex, goToScene, false);
                StartCoroutine(loadSceneWithLoadingScreenWithText(2));
            }

            else
            {
                if(currentQuest != null)
                {
                    dungeonEnteringPreventedUI.SetActive(true);
                    dungeonEnteringPreventedUI.GetComponent<DungeonEnteringPreventedUI>().SetUIContent(currentQuest);
                }
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
    IEnumerator loadSceneWithLoadingScreenWithText(int sceneId)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneId);
        loadingScreen.SetActive(true);
        while (!operation.isDone)
        {
            float progressValue = Mathf.Clamp01(operation.progress / 0.9f);
            if (progressValue < 0.33f)
            {
                loadingScreenText.text = "Loading.";
            }
            else if (progressValue < 0.66f)
            {
                loadingScreenText.text = "Loading..";
            }
            else if (progressValue > 0.66f)
            {
                loadingScreenText.text = "Loading...";
            }
            yield return null;
        }
    }
}
