using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class TalkToBearQuestStep : QuestStep
{
    [SerializeField] DialogueVariables dialogueToPassForProgress;
    [SerializeField]Animator cinematicAnimator;
    public UnityEvent animationEvent;
    private bool talkedToBear; // this might not be needed here, but to avoid any errors in the other quest code (state of the quest etc.), there's some value to be saved

    private void OnEnable()
    {
        GameEventsManager.instance.dialogueEvents.OnChangeDialogueVariable += CheckProgressInDialogue;
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
        GameEventsManager.instance.dialogueEvents.OnChangeDialogueVariable -= CheckProgressInDialogue;
        GameEventsManager.instance.questEvents.OnAdvanceQuest -= PlayIntroCinematic;

        SceneManager.sceneLoaded -= UpdateQuestUI;
    }

    private void Start()
    {
        GameEventsManager.instance.questEvents.ShowQuestUI(GetQuestId(), objective, progress);
    }

    private void UpdateQuestUI(Scene scene, LoadSceneMode mode)
    {
        if (scene.name.Contains("Overworld", System.StringComparison.CurrentCultureIgnoreCase) || scene.name.Contains("Tutorial", System.StringComparison.CurrentCultureIgnoreCase))
        {
            GameEventsManager.instance.questEvents.ShowQuestUI(GetQuestId(), objective, progress);

            try
            {
                cinematicAnimator = GameObject.Find("IntroCamera").GetComponent<Animator>();
            }
            catch (System.Exception)
            {
                cinematicAnimator = null;
            }
        }     
    }

    private void CheckProgressInDialogue(DialogueVariables changedVariable)
    {
        if(changedVariable == dialogueToPassForProgress)
        {
            FinishQuestStep();
        }
    }
    private void PlayIntroCinematic(string name)
    {
        if(name == "Tutorial")
        {
            try
            {
                if (gameObject != null)
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