using UnityEngine;
using UnityEngine.Serialization;

public class EnableBearDialogueAfterTutorial : MonoBehaviour
{
    [FormerlySerializedAs("tutorialQuest")] [SerializeField] private QuestScriptableObject _tutorialQuest;
    [FormerlySerializedAs("npcDialogue")] [SerializeField] private NpcDialogue _npcDialogue;

    private void Start()
    {
        EnableDialogue(_tutorialQuest.id);

        GameEventsManager.instance.QuestEvents.OnFinishQuest += EnableDialogue;
    }

    private void OnDisable()
    {

        GameEventsManager.instance.QuestEvents.OnFinishQuest -= EnableDialogue;
    }

    private void EnableDialogue(string id)
    {
        QuestState state = QuestManager.Instance.CheckQuestState(id);

        if (state == QuestState.Finished)
        {
            _npcDialogue.enabled = true;
        }
    }
}