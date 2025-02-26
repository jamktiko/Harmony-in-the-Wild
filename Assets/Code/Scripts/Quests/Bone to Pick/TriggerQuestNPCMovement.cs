using UnityEngine;
using UnityEngine.SceneManagement;

public class TriggerQuestNPCMovement : MonoBehaviour
{
    [SerializeField] private DialogueQuestNPCs characterToMove;

    private void Start()
    {
        GameEventsManager.instance.questEvents.StartMovingQuestNPC(characterToMove);
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += TriggerMovement;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= TriggerMovement;
    }

    private void TriggerMovement(Scene scene, LoadSceneMode mode)
    {
        if (scene.name.Contains("Overworld", System.StringComparison.CurrentCultureIgnoreCase))
        {
            GameEventsManager.instance.questEvents.StartMovingQuestNPC(characterToMove);
        }
    }
}
