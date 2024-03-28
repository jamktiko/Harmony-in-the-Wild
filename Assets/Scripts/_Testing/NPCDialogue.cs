using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCDialogue : MonoBehaviour
{
    [SerializeField] private List<NPCDialogueState> dialogueOptions;

    [Header("Debugging")]
    [SerializeField] private QuestScriptableObject latestQuest;

    private bool playerIsNear;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.E) && playerIsNear)
        {
            SetDialogueToPlay();
        }
    }

    private void SetDialogueToPlay()
    {
        //NOTE! fetch latest dungeon quest here

        if(latestQuest == null)
        {
            DialogueManager.instance.StartDialogue(dialogueOptions[0].dialogueToPlay);
            return;
        }

        for(int i = 1; i < dialogueOptions.Count; i++)
        {
            if(dialogueOptions[i].questSO == latestQuest)
            {
                DialogueManager.instance.StartDialogue(dialogueOptions[i].dialogueToPlay);
                return;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerIsNear = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerIsNear = false;
        }
    }

    [System.Serializable]
    public class NPCDialogueState
    {
        public QuestScriptableObject questSO;
        public TextAsset dialogueToPlay;
    }
}
