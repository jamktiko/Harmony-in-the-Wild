using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(BoxCollider))]
public class DungeonEntrance : MonoBehaviour
{
    [Header("Quest & Ability")]
    [SerializeField] private QuestScriptableObject dungeonQuest;
    [SerializeField] private Abilities abilityGrantedForDungeon;

    [Header("Needed References")]
    [SerializeField] private GameObject dungeonEnteringPreventedUI;

    [Header("Config")]
    [SerializeField] private SceneManagerHelper.Scene learningStage;
    [SerializeField] private SceneManagerHelper.Scene bossStage;
    [SerializeField] private int storybookSectionIndex;
    [Tooltip("Tick if a quest is started when entering this dungeon")]
    [SerializeField] private Transform respawnPoint;

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

            currentQuest = QuestManager.instance.GetQuestById(questId);

            currentQuestState = currentQuest.state;
        }
    }

    private void OnEnable()
    {
        GameEventsManager.instance.questEvents.OnQuestStateChange += QuestStateChange;
    }

    private void OnDisable()
    {
        GameEventsManager.instance.questEvents.OnQuestStateChange -= QuestStateChange;
    }

    private void OnTriggerEnter(Collider other)
    {
        // TODO: Fire event to save last known coordinates in overworld.
        // Either as Vector3 and persist in PlayerManager?
        // Or save to savefile as list of floats?

        if (other.gameObject.CompareTag("Trigger"))
        {
            if(currentQuestState == QuestState.CAN_START)
            {
                AbilityManager.instance.UnlockAbility(abilityGrantedForDungeon);
                Debug.Log("Ability " + abilityGrantedForDungeon + " granted for dungeon entrance.");
                GameEventsManager.instance.questEvents.StartQuest(questId);

                RespawnManager.instance.SetRespawnPosition(respawnPoint.position);

                StorybookHandler.instance.SetNewStorybookData(storybookSectionIndex, SceneManagerHelper.GetSceneName(learningStage), false);

                GameEventsManager.instance.uiEvents.ShowLoadingScreen("Storybook");
            }

            else if (currentQuestState == QuestState.IN_PROGRESS)
            {
                int currentQuestStepIndex = QuestManager.instance.GetQuestById(dungeonQuest.id).GetCurrentQuestStepIndex();
                AbilityManager.instance.UnlockAbility(abilityGrantedForDungeon);
                RespawnManager.instance.SetRespawnPosition(respawnPoint.position);

                if (currentQuestStepIndex == 0)
                {
                    StorybookHandler.instance.SetNewStorybookData(storybookSectionIndex, SceneManagerHelper.GetSceneName(learningStage), false);
                }

                else
                {
                    StorybookHandler.instance.SetNewStorybookData(storybookSectionIndex, SceneManagerHelper.GetSceneName(bossStage), false);
                }

                GameEventsManager.instance.uiEvents.ShowLoadingScreen("Storybook");
            }

            else if (currentQuestState == QuestState.FINISHED)
            {
                RespawnManager.instance.SetRespawnPosition(respawnPoint.position);

                // add possible storybook config here & change goToScene to Storybook scene
                StorybookHandler.instance.SetNewStorybookData(storybookSectionIndex, SceneManagerHelper.GetSceneName(learningStage), false);

                GameEventsManager.instance.uiEvents.ShowLoadingScreen("Storybook");
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
            //Debug.Log("Quest with id: " + questId + " updated to state: " + currentQuestState);
        }
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
