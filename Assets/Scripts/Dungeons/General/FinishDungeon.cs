using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishDungeon : MonoBehaviour
{
    public const string StorybookSceneName = "Storybook";

    [Header("Config")]
    [SerializeField] private QuestScriptableObject questSO;
    [SerializeField] private int stageIndex;
    [SerializeField] private int gainedAbilityIndex; //NOTE: Is this used?
    [SerializeField] private int storybookSectionIndex;
    [SerializeField] private string goToScene;

    private AudioSource audioSource; //NOTE: USe descriptive name
    private string questId;

    private void Start()
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
            if (questSO != null)
            {
                GameEventsManager.instance.questEvents.AdvanceDungeonQuest(questId, stageIndex);
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
    }
}
