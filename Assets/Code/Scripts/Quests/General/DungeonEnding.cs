using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

public class DungeonEnding : MonoBehaviour
{
    public const string StorybookSceneName = "Storybook";

    [FormerlySerializedAs("questSO")]
    [Header("Config")]
    [SerializeField] private QuestScriptableObject _questSo;
    [FormerlySerializedAs("storybookSectionIndex")] [SerializeField] private int _storybookSectionIndex;
    [FormerlySerializedAs("goToScene")] [SerializeField] private SceneManagerHelper.Scene _goToScene = SceneManagerHelper.Scene.Overworld;

    private AudioSource _audioSource; //NOTE: USe descriptive name
    private string _questId;

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();

        if (_questSo != null)
        {
            _questId = _questSo.id;
        }

        else
        {
            Debug.LogWarning("No quest SO assigned to trigger the ending the dungeon!");
        }
    }

    private void OnEnable()
    {
        //GameEventsManager.instance.questEvents.OnFinishQuest += TriggerSceneTransition;
        GameEventsManager.instance.QuestEvents.OnQuestStateChange += TriggerSceneTransition;
    }

    private void OnDisable()
    {
        //GameEventsManager.instance.questEvents.OnFinishQuest -= TriggerSceneTransition;
        GameEventsManager.instance.QuestEvents.OnQuestStateChange -= TriggerSceneTransition;
    }

    private void TriggerSceneTransition(Quest quest)
    {
        if (quest.Info.id == _questId && quest.State == QuestState.CanFinish)
        {
            Debug.Log("Corresponding dungeon quest finished...");
            StartCoroutine(ShowDungeonCompletedStorybook());
        }

        else
        {
            Debug.LogError("ID not matching for the current quest: " + quest.Info.id);
        }
    }

    private IEnumerator ShowDungeonCompletedStorybook()
    {
        Debug.Log("Playing audio for dungeon completion and preparing to change scene...");
        _audioSource.Play();

        yield return new WaitForSeconds(_audioSource.clip.length + 0.5f);

        StorybookHandler.Instance.SetNewStorybookData(_storybookSectionIndex, _goToScene, false);
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
