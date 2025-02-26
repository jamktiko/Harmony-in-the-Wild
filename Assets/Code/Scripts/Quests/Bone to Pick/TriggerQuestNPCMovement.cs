using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class TriggerQuestNpcMovement : MonoBehaviour
{
    [FormerlySerializedAs("characterToMove")] [SerializeField] private DialogueQuestNpCs _characterToMove;

    private void Start()
    {
        GameEventsManager.instance.QuestEvents.StartMovingQuestNpc(_characterToMove);
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
            GameEventsManager.instance.QuestEvents.StartMovingQuestNpc(_characterToMove);
        }
    }
}
