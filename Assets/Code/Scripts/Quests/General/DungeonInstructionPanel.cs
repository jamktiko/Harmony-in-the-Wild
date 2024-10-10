using UnityEngine;
using UnityEngine.VFX;

public class DungeonInstructionPanel : MonoBehaviour
{
    [SerializeField] private VisualEffect dungeonEntranceVFX;
    [SerializeField] private DungeonQuestDialogue dungeonQuestDialogue;
    [SerializeField] private GameObject closingInstructions;
    private int onDungeonStartID;
    private float timeToEnableHidingInstructions = 3f;
    private bool canHideInstructions = false;

    private void Start()
    {
        // make the player stop moving through dialogue events
        GameEventsManager.instance.dialogueEvents.StartDialogue();

        onDungeonStartID = Shader.PropertyToID("OnDungeonStart");

        // Check if the dungeonEntranceVFX is not assigned in the inspector
        if (dungeonEntranceVFX == null)
        {
            // Find the DungeonEntrance VFX by its name in the hierarchy
            var dungeonEntranceObject = GameObject.Find("DungeonEntrance");
            if (dungeonEntranceObject != null)
            {
                dungeonEntranceVFX = dungeonEntranceObject.GetComponent<VisualEffect>();

                if (dungeonEntranceVFX == null)
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

        Invoke(nameof(EnableHidingInstructions), timeToEnableHidingInstructions);
    }

    void Update()
    {
        if (PlayerInputHandler.instance.CloseUIInput.WasPerformedThisFrame() && canHideInstructions)
        {
            Invoke(nameof(HideInstructionPanel), 0.1f);

            if (dungeonEntranceVFX != null)
            {
                dungeonEntranceVFX.SendEvent("OnDungeonStart");
            }

            else
            {
                Debug.LogWarning("dungeonEntranceVFX variable not assigned.");
            }
        }
    }

    private void HideInstructionPanel()
    {
        gameObject.SetActive(false);

        // enable player movement through dialogue events
        GameEventsManager.instance.dialogueEvents.EndDialogue();

        if (dungeonQuestDialogue != null)
        {
            dungeonQuestDialogue.PlayStartDungeonDialogue();
        }

        else
        {
            Debug.LogWarning("No Dungeon Quest Dialogue component assigned to Info Board. Please check inspector!");
        }
    }

    private void EnableHidingInstructions()
    {
        canHideInstructions = true;
        closingInstructions.SetActive(true);
    }
}
