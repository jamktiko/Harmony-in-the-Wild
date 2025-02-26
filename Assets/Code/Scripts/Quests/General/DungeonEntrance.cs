using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(BoxCollider))]
public class DungeonEntrance : MonoBehaviour
{
    [FormerlySerializedAs("dungeonQuest")]
    [Header("Quest & Ability")]
    [SerializeField] private QuestScriptableObject _dungeonQuest;
    [FormerlySerializedAs("abilityGrantedForDungeon")] [SerializeField] private Abilities _abilityGrantedForDungeon;

    [FormerlySerializedAs("dungeonEnteringPreventedUI")]
    [Header("Needed References")]
    [SerializeField] private GameObject _dungeonEnteringPreventedUI;

    [FormerlySerializedAs("learningStage")]
    [Header("Config")]
    [SerializeField] private SceneManagerHelper.Scene _learningStage;
    [FormerlySerializedAs("bossStage")] [SerializeField] private SceneManagerHelper.Scene _bossStage;
    [FormerlySerializedAs("storybookSectionIndex")] [SerializeField] private int _storybookSectionIndex;
    [FormerlySerializedAs("respawnPoint")]
    [Tooltip("Tick if a quest is started when entering this dungeon")]
    [SerializeField] private Transform _respawnPoint;

    private string _questId;
    private QuestState _currentQuestState;
    private Quest _currentQuest;

    private void Start()
    {
        if (_dungeonQuest != null)
        {
            _questId = _dungeonQuest.id;

            _currentQuest = QuestManager.Instance.GetQuestById(_questId);

            _currentQuestState = _currentQuest.State;
        }
    }

    private void OnEnable()
    {
        GameEventsManager.instance.QuestEvents.OnQuestStateChange += QuestStateChange;
    }

    private void OnDisable()
    {
        GameEventsManager.instance.QuestEvents.OnQuestStateChange -= QuestStateChange;
    }

    private void OnTriggerEnter(Collider other)
    {
        // TODO: Fire event to save last known coordinates in overworld.
        // Either as Vector3 and persist in PlayerManager?
        // Or save to savefile as list of floats?

        if (other.gameObject.CompareTag("Trigger"))
        {
            if (_currentQuestState == QuestState.CanStart)
            {
                AbilityManager.Instance.UnlockAbility(_abilityGrantedForDungeon);
                Debug.Log("Ability " + _abilityGrantedForDungeon + " granted for dungeon entrance.");
                GameEventsManager.instance.QuestEvents.StartQuest(_questId);

                RespawnManager.Instance.SetRespawnPosition(_respawnPoint.position);

                StorybookHandler.Instance.SetNewStorybookData(_storybookSectionIndex, _learningStage, false);

                GameEventsManager.instance.UIEvents.ShowLoadingScreen(SceneManagerHelper.Scene.Storybook);
            }

            else if (_currentQuestState == QuestState.InProgress)
            {
                int currentQuestStepIndex = QuestManager.Instance.GetQuestById(_dungeonQuest.id).GetCurrentQuestStepIndex();
                AbilityManager.Instance.UnlockAbility(_abilityGrantedForDungeon);
                RespawnManager.Instance.SetRespawnPosition(_respawnPoint.position);

                if (currentQuestStepIndex == 0)
                {
                    StorybookHandler.Instance.SetNewStorybookData(_storybookSectionIndex, _learningStage, false);
                }

                else
                {
                    StorybookHandler.Instance.SetNewStorybookData(_storybookSectionIndex, _bossStage, false);
                }

                GameEventsManager.instance.UIEvents.ShowLoadingScreen(SceneManagerHelper.Scene.Storybook);
            }

            else if (_currentQuestState == QuestState.Finished || _currentQuestState == QuestState.CanFinish)
            {
                RespawnManager.Instance.SetRespawnPosition(_respawnPoint.position);

                // add possible storybook config here & change goToScene to Storybook scene
                StorybookHandler.Instance.SetNewStorybookData(_storybookSectionIndex, _learningStage, false);

                GameEventsManager.instance.UIEvents.ShowLoadingScreen(SceneManagerHelper.Scene.Storybook);
            }

            else
            {
                if (_currentQuest != null)
                {
                    _dungeonEnteringPreventedUI.SetActive(true);
                    _dungeonEnteringPreventedUI.GetComponent<DungeonEnteringPreventedUI>().SetUIContent(_currentQuest);
                }
            }
        }
    }

    private void QuestStateChange(Quest quest)
    {
        // only update the quest state if this point has the corresponding quest
        if (quest.Info.id.Equals(_questId))
        {
            _currentQuestState = quest.State;
            //Debug.Log("Quest with id: " + questId + " updated to state: " + currentQuestState);
        }
    }
}
