using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoToDialogueScene : MonoBehaviour
{
    Interactable interactable;
    // Start is called before the first frame update
    private void Start()
    {
        
        interactable=gameObject.GetComponent<Interactable>();
    }
    // Update is called once per frame
    void Update()
    {
        if (QuestManager.instance.CheckQuestState("BunnyQuest").Equals(QuestState.FINISHED)&&!interactable.enabled)
        {
            interactable.enabled = true;
            
        }
        if (interactable.used && interactable.enabled)
        {
            Cursor.lockState = CursorLockMode.None;
            SceneManager.LoadScene(1);
        }
    }
}
