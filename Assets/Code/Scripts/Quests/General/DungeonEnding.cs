using System.Collections;
using UnityEngine;

public class DungeonEnding : MonoBehaviour
{
    public const string StorybookSceneName = "Storybook";

    [Header("Config")]
    [SerializeField] private QuestScriptableObject questSO;
    [SerializeField] private int storybookSectionIndex;
    [SerializeField] private SceneManagerHelper.Scene goToScene = SceneManagerHelper.Scene.Overworld;

    private AudioSource audioSource; //NOTE: USe descriptive name
    private string questId;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();

        if (questSO != null)
        {
            questId = questSO.id;
        }

        else
        {
            Debug.LogWarning("No quest SO assigned to trigger the ending the dungeon!");
        }
    }

    private void OnEnable()
    {
        //GameEventsManager.instance.questEvents.OnFinishQuest += TriggerSceneTransition;
        GameEventsManager.instance.questEvents.OnQuestStateChange += TriggerSceneTransition;
    }

    private void OnDisable()
    {
        //GameEventsManager.instance.questEvents.OnFinishQuest -= TriggerSceneTransition;
        GameEventsManager.instance.questEvents.OnQuestStateChange -= TriggerSceneTransition;
    }

    private void TriggerSceneTransition(Quest quest)
    {
        if (quest.info.id == questId && quest.state == QuestState.CAN_FINISH)
        {
            Debug.Log("Corresponding dungeon quest finished...");
            StartCoroutine(ShowDungeonCompletedStorybook());
        }

        else
        {
            Debug.LogError("ID not matching for the current quest: " + quest.info.id);
        }
    }

    private IEnumerator ShowDungeonCompletedStorybook()
    {
        Debug.Log("Playing audio for dungeon completion and preparing to change scene...");
        audioSource.Play();

        yield return new WaitForSeconds(audioSource.clip.length + 0.5f);

        StorybookHandler.instance.SetNewStorybookData(storybookSectionIndex, goToScene, false);
        SceneManagerHelper.LoadScene(SceneManagerHelper.Scene.Storybook);
    }


    /*private void Start()
    {
        audioSource = GetComponent<AudioSource>();

        if(questSO != null)
        {
            questId = questSO.id;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other);
        Debug.Log(other.tag);
        if (other.gameObject.CompareTag("Trigger"))
        {
            SaveManager.instance.SaveGame();

            if (questSO != null)
            {
                GameEventsManager.instance.questEvents.AdvanceDungeonQuest(questId);
                QuestManager.instance.RequestFinishQuest(questId);
            }
            StartCoroutine(ShowDungeonCompletedStorybook());
        }
    }

    private IEnumerator ShowDungeonCompletedStorybook()
    {
        audioSource.Play();
        //if (questSO != null)
        //{
        //    GameEventsManager.instance.questEvents.AdvanceDungeonQuest(questId, stageIndex);
        //}
        yield return new WaitForSeconds(audioSource.clip.length + 0.5f);


        StorybookHandler.instance.SetNewStorybookData(storybookSectionIndex, goToScene, false);
        SceneManager.LoadScene(StorybookSceneName);
    }*/
}
