using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class TalkToBearQuestStep : QuestStep
{
    [FormerlySerializedAs("dialogueToPassForProgress")] [SerializeField]
    private DialogueVariables _dialogueToPassForProgress;
    [FormerlySerializedAs("cinematicAnimator")] [SerializeField]
    private Animator _cinematicAnimator;
    [FormerlySerializedAs("animationEvent")] public UnityEvent AnimationEvent;
    private DialogueQuestNpCs _character = DialogueQuestNpCs.Bear;
    private bool _talkedToBear; // this might not be needed here, but to avoid any errors in the other quest code (state of the quest etc.), there's some value to be saved
    private bool _canProgressQuest = false;

    private void OnEnable()
    {
        GameEventsManager.instance.DialogueEvents.OnChangeDialogueVariable += CheckProgressInDialogue;
        GameEventsManager.instance.QuestEvents.OnAdvanceQuest += PlayIntroCinematic;
        GameEventsManager.instance.DialogueEvents.OnRegisterPlayerNearNpc += PlayerIsClose;

        SceneManager.sceneLoaded += UpdateQuestUI;

        try
        {
            _cinematicAnimator = GameObject.Find("IntroCamera").GetComponent<Animator>();
        }
        catch (System.Exception)
        {
            _cinematicAnimator = null;
        }

    }

    private void OnDisable()
    {
        GameEventsManager.instance.DialogueEvents.OnChangeDialogueVariable -= CheckProgressInDialogue;
        GameEventsManager.instance.QuestEvents.OnAdvanceQuest -= PlayIntroCinematic;
        GameEventsManager.instance.DialogueEvents.OnRegisterPlayerNearNpc -= PlayerIsClose;

        SceneManager.sceneLoaded -= UpdateQuestUI;
    }

    private void Start()
    {
        GameEventsManager.instance.QuestEvents.ShowQuestUI(GetQuestId(), Objective, Progress);
    }

    private void UpdateQuestUI(Scene scene, LoadSceneMode mode)
    {
        if (scene.name.Contains("Overworld", System.StringComparison.CurrentCultureIgnoreCase) || scene.name.Contains("Tutorial", System.StringComparison.CurrentCultureIgnoreCase))
        {
            GameEventsManager.instance.QuestEvents.ShowQuestUI(GetQuestId(), Objective, Progress);

            try
            {
                _cinematicAnimator = GameObject.Find("IntroCamera").GetComponent<Animator>();
            }
            catch (System.Exception)
            {
                _cinematicAnimator = null;
            }
        }
    }

    private void CheckProgressInDialogue(DialogueVariables changedVariable)
    {
        if (changedVariable == _dialogueToPassForProgress && _canProgressQuest)
        {
            FinishQuestStep();
        }
    }

    private void PlayerIsClose(DialogueQuestNpCs npc, bool isClose)
    {
        if (npc == _character)
        {
            Debug.Log("Toggling quest progress for tutorial: " + isClose);
            _canProgressQuest = isClose;
        }

        else
        {
            Debug.Log("Character not matching for tutorial progress.");
        }
    }

    private void PlayIntroCinematic(string name)
    {
        if (name == "Tutorial")
        {
            try
            {
                if (gameObject != null)
                {
                    if (gameObject.name.Contains("07"))
                    {
                        _cinematicAnimator.enabled = true;
                        AnimationEvent.Invoke();
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
        string state = _talkedToBear.ToString();
        ChangeState(state);
    }

    protected override void SetQuestStepState(string state)
    {
        _talkedToBear = System.Convert.ToBoolean(state);

        UpdateState();
    }
}