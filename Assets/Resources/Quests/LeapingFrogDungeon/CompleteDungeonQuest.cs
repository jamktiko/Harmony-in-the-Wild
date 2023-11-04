using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// NOTE! TURN THIS INTO QUEST STEP!!
public class CompleteDungeonQuest : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] private QuestScriptableObject questScriptableObject;

    private string questId;

    private void Start()
    {
        questId = questScriptableObject.id;
    }

    public void CompleteDungeon()
    {

    }
}
