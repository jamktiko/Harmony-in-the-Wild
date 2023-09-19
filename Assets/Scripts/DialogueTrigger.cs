using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [SerializeField] private TextAsset inkJSON;

    // NOTE FOR JUTTA
    // NOTE EDIT THIS SO THAT THE DIALOGUE IS TRIGGERED BY PRESSING "E" WHEN CLOSE ENOUGH TO THE TARGET

    public void TriggerDialogue()
    {
        DialogueManager.GetInstance().StartDialogue(inkJSON);
    }
}
