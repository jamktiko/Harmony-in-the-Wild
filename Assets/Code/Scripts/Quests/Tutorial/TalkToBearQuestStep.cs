using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class TalkToBearQuestStep : QuestStep
{
    [Tooltip("Target index for the completed dialogue. Checking if the dialogue with this quest step has been completed.")]
    [SerializeField] private int targetDialogueIndex;
    [SerializeField]Animator cinematicAnimator;
    public UnityEvent animationEvent;
    private bool talkedToBear; // this might not be needed here, but to avoid any errors in the other quest code (state of the quest etc.), there's some value to be saved

    private void OnEnable()
    {
        GameEventsManager.instance.dialogueEvents.OnEndDialogue += CheckProgressInDialogue;
        GameEventsManager.instance.questEvents.OnAdvanceQuest += PlayIntroCinematic;

        SceneManager.sceneLoaded += UpdateQuestUI;

        try
        {
            cinematicAnimator = GameObject.Find("IntroCamera").GetComponent<Animator>();
        }
        catch (System.Exception)
        {
            cinematicAnimator = null;
        }
        
    }

    private void OnDisable()
    {
        GameEventsManager.instance.dialogueEvents.OnEndDialogue -= CheckProgressInDialogue;
        GameEventsManager.instance.questEvents.OnAdvanceQuest -= PlayIntroCinematic;

        SceneManager.sceneLoaded -= UpdateQuestUI;
    }

    private void Start()
    {
        GameEventsManager.instance.questEvents.ShowQuestUI(GetQuestId(), objective, progress);
    }

    private void UpdateQuestUI(Scene scene, LoadSceneMode mode)
    {
        if (scene.name.Contains("Overworld", System.StringComparison.CurrentCultureIgnoreCase))
        {
            GameEventsManager.instance.questEvents.ShowQuestUI(GetQuestId(), objective, progress);
        }     
    }

    private void CheckProgressInDialogue()
    {
        Invoke(nameof(FetchDialogueData),0.01f);
    }
    private void PlayIntroCinematic(string name)
    {
        try
        {
            if (gameObject!=null)
            {
            if (gameObject.name.Contains("07"))
            {
                cinematicAnimator.enabled = true;
                    animationEvent.Invoke();
            }
            }
        }
        catch (System.Exception)
        {
        }
        
    }

    private void FetchDialogueData()
    {
        // check the latest completed dialogue from Ink
        int latestCompletedDialogue = ((Ink.Runtime.IntValue)DialogueManager.instance.GetDialogueVariableState("latestTutorialQuestStepDialogueCompleted")).value;

        Debug.Log("Latest completed dialogue: " + latestCompletedDialogue + ", target dialogue: " + targetDialogueIndex);

        // if the current value matches the target value, finish the quest step
        if (latestCompletedDialogue == targetDialogueIndex)
        {
            FinishQuestStep();
        }
    }

    private void UpdateState()
    {
        string state = talkedToBear.ToString();
        ChangeState(state);
    }

    protected override void SetQuestStepState(string state)
    {
        talkedToBear = System.Convert.ToBoolean(state);

        UpdateState();
    }
}