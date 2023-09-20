using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class QuestStep : MonoBehaviour
{
    private bool isFinished =false;
    // Start is called before the first frame update
    private string questId;

    public void InitializeQuestStep(string id)
    {
        questId = id;
    }
    protected void FinishQuestStep()
    {
        if (!isFinished) 
        { 
            isFinished = true;
            Destroy(gameObject);
        }
    }
}
