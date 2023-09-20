using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quest
{
    public QuestScriptableObject info;

    public QuestState state;

    private int currentQuestStepIndex;

    public Quest(QuestScriptableObject questInfo)
    {
        info = questInfo;
        state = QuestState.REQUIREMENTS_NOT_MET;
        currentQuestStepIndex = 0;
    }

    public void MoveToNextStep()
    {
        currentQuestStepIndex++;


    }

    public bool CurrentStepExists()
    {
        return currentQuestStepIndex < info.QuestStepPrefabs.Length;
    }

    public void InstantiateCurrentQuestStep(Transform parentTransform)
    {
        GameObject questStepPrefab = GetCurrentQuestStepPrefab();

        if(questStepPrefab != null)
        {
            QuestStep questStep = Object.Instantiate(questStepPrefab, parentTransform).GetComponent<QuestStep>();
            questStep.InitializeQuestStep(info.id);
        }
    }

    private GameObject GetCurrentQuestStepPrefab()
    {
        GameObject questStepPrefab = null;

        if (CurrentStepExists())
        {
            questStepPrefab = info.QuestStepPrefabs[currentQuestStepIndex];
        }

        else
        {
            Debug.LogWarning("Quest step index out of range.");
        }

        return questStepPrefab;
    }
}
