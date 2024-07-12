using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishDungeonQuestStepWithTrigger : MonoBehaviour
{
    [SerializeField] private QuestScriptableObject dungeonQuest;

    [Header("For Learning Stages Only")]
    [SerializeField] private bool isLearningStage;
    [SerializeField] private SceneManagerHelper.Scene nextScene;

    private bool canFinishQuest;
    private string questId;

    private void Start()
    {
        questId = dungeonQuest.id;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Detected a collider on door");

        if(canFinishQuest && other.CompareTag("Trigger"))
        {
            Debug.Log("Advancing in dungeon quest");
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
        Debug.Log("Enabled interaction with final door");
    }
}
