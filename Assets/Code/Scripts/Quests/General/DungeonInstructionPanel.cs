using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.VFX;

public class DungeonInstructionPanel : MonoBehaviour
{
    [FormerlySerializedAs("dungeonEntranceVFX")] [SerializeField] private VisualEffect _dungeonEntranceVFX;
    [FormerlySerializedAs("dungeonQuestDialogue")] [SerializeField] private DungeonQuestDialogue _dungeonQuestDialogue;
    [FormerlySerializedAs("closingInstructions")] [SerializeField] private GameObject _closingInstructions;
    private int _onDungeonStartID;
    private float _timeToEnableHidingInstructions = 4f;
    private bool _canHideInstructions = false;

    private void Start()
    {
        // make the player stop moving through dialogue events
        Invoke(nameof(CallDialogueEvent), 0.1f);

        _onDungeonStartID = Shader.PropertyToID("OnDungeonStart");

        // Check if the dungeonEntranceVFX is not assigned in the inspector
        if (_dungeonEntranceVFX == null)
        {
            // Find the DungeonEntrance VFX by its name in the hierarchy
            var dungeonEntranceObject = GameObject.Find("DungeonEntrance");
            if (dungeonEntranceObject != null)
            {
                _dungeonEntranceVFX = dungeonEntranceObject.GetComponent<VisualEffect>();

                if (_dungeonEntranceVFX == null)
                {
                    Debug.LogWarning("VisualEffect component not found on DungeonEntrance object!");
                }
            }
            else
            {
                Debug.LogWarning("DungeonEntrance object not found in the hierarchy!");
            }
        }
        //Debug.Log(PlayerInputHandler.instance.playerInput.currentActionMap);

        Invoke(nameof(EnableHidingInstructions), _timeToEnableHidingInstructions);
    }

    private void Update()
    {
        if (PlayerInputHandler.Instance.CloseUIInput.WasPerformedThisFrame() && _canHideInstructions)
        {
            Invoke(nameof(HideInstructionPanel), 0.1f);

            if (_dungeonEntranceVFX != null)
            {
                _dungeonEntranceVFX.SendEvent("OnDungeonStart");
            }

            else
            {
                Debug.LogWarning("dungeonEntranceVFX variable not assigned.");
            }
        }
    }

    private void CallDialogueEvent()
    {
        GameEventsManager.instance.DialogueEvents.StartDialogue();
    }

    private void HideInstructionPanel()
    {
        gameObject.SetActive(false);

        // enable player movement through dialogue events
        GameEventsManager.instance.DialogueEvents.EndDialogue();
        GameEventsManager.instance.UIEvents.HideInstructionPanel();

        if (_dungeonQuestDialogue != null)
        {
            _dungeonQuestDialogue.PlayStartDungeonDialogue();
        }

        else
        {
            Debug.LogWarning("No Dungeon Quest Dialogue component assigned to Info Board. Please check inspector!");
        }
    }

    private void EnableHidingInstructions()
    {
        _canHideInstructions = true;
        _closingInstructions.SetActive(true);
    }
}
