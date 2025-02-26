using UnityEngine;
using UnityEngine.SceneManagement;

public class TalkToNPCQuestStep : QuestStep
{
    [SerializeField] DialogueVariables dialogueToPassForProgress;
    [SerializeField] private int midQuestDialogueIndex = 0;
    [SerializeField] private DialogueQuestNPCs character = DialogueQuestNPCs.Whale;
    private bool canProgressQuest;

    private void Start()
    {
        GameEventsManager.instance.questEvents.ShowQuestUI(GetQuestId(), objective, progress);
        GameEventsManager.instance.dialogueEvents.SetMidQuestDialogue(midQuestDialogueIndex, GetQuestId());
    }

    private void OnEnable()
    {
        GameEventsManager.instance.dialogueEvents.OnChangeDialogueVariable += CheckProgressInDialogue;
        GameEventsManager.instance.dialogueEvents.OnRegisterPlayerNearNPC += PlayerIsClose;
        SceneManager.sceneLoaded += SetInfoInOverworld;
    }

    private void OnDisable()
    {
        GameEventsManager.instance.dialogueEvents.OnChangeDialogueVariable -= CheckProgressInDialogue;
        GameEventsManager.instance.dialogueEvents.OnRegisterPlayerNearNPC -= PlayerIsClose;
        SceneManager.sceneLoaded -= SetInfoInOverworld;
    }

    private void SetInfoInOverworld(Scene scene, LoadSceneMode mode)
    {
        if (scene.name.Contains("Overworld", System.StringComparison.CurrentCultureIgnoreCase))
        {
            GameEventsManager.instance.dialogueEvents.SetMidQuestDialogue(midQuestDialogueIndex, GetQuestId());
            GameEventsManager.instance.questEvents.ShowQuestUI(GetQuestId(), objective, progress);
        }
    }

    private void CheckProgressInDialogue(DialogueVariables changedVariable)
    {
        if (changedVariable == dialogueToPassForProgress && canProgressQuest)
        {
            FinishQuestStep();
        }

        else
        {
            Debug.Log("Not able to finish the quest step, values not matching.");
        }
    }

    private void PlayerIsClose(DialogueQuestNPCs npc, bool isClose)
    {
        if (npc == character)
        {
            canProgressQuest = isClose;
        }
    }

    private void UpdateState()
    {
        // nothing to update
    }

    protected override void SetQuestStepState(string state)
    {
        // nothing to update
    }
}