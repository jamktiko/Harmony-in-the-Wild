using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [SerializeField] private TextAsset inkJSON;

    public void TriggerDialogue()
    {
        DialogueManager.GetInstance().StartDialogue(inkJSON);
    }
}
