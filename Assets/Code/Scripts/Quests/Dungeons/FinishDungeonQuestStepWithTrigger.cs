using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishDungeonQuestStepWithTrigger : MonoBehaviour
{
    [SerializeField] private QuestScriptableObject dungeonQuest;
    [Tooltip("If this box is ticked, you can immediately interact with the object without any other conditions as prerequisities.")]
    [SerializeField] private bool enableInteractionFromStart;

    [Header("For Learning Stages Only")]
    [SerializeField] private bool isLearningStage;
    [SerializeField] private SceneManagerHelper.Scene nextScene;

    private bool canFinishQuest;
    private string questId;

    private void Start()
    {
        questId = dungeonQuest.id;

        if (enableInteractionFromStart)
        {
            canFinishQuest = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!canFinishQuest)
        {
            Debug.LogWarning("Interaction with the transition object has not been enabled yet. Check if enabling it" +
                "has been called from other scripts handling the enabling conditions (for example, collecting items) " +
                "or that Enable Interaction From Start has been ticked.");
            return;
        }

        if(canFinishQuest && other.CompareTag("Trigger"))
        {
            GameEventsManager.instance.questEvents.AdvanceDungeonQuest(questId);
            
            if (isLearningStage)
            {
                Invoke(nameof(ChangeScene), 0.15f);
            }
        }
    }

    private void ChangeScene()
    {
        SceneManagerHelper.LoadScene(nextScene);
    }

    public void EnableInteraction()
    {
        canFinishQuest = true;
    }
}
