using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableBearDialogueAfterTutorial : MonoBehaviour
{
    [SerializeField] private QuestScriptableObject tutorialQuest;
    [SerializeField] private NPC_Dialogue npcDialogue;

    private void Start()
    {
        EnableDialogue(tutorialQuest.id);

        GameEventsManager.instance.questEvents.OnFinishQuest += EnableDialogue;
    }

    private void OnDisable()
    {

        GameEventsManager.instance.questEvents.OnFinishQuest -= EnableDialogue;
    }

    private void EnableDialogue(string id)
    {
        QuestState state = QuestManager.instance.CheckQuestState(id);

        if (state == QuestState.FINISHED)
        {
            npcDialogue.enabled = true;
        }
    }
}