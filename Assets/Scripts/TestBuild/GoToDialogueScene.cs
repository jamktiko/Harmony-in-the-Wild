using UnityEngine;
using UnityEngine.SceneManagement;

public class GoToDialogueScene : MonoBehaviour
{
    Interactable interactable;

    private void Start()
    {
        
        interactable=gameObject.GetComponent<Interactable>();
    }

    void Update()
    {
        if (QuestManager.instance.CheckQuestState("BunnyQuest").Equals(QuestState.FINISHED)&&!interactable.enabled)
        {
            interactable.enabled = true;
            
        }
        if (interactable.wasUsed && interactable.enabled)
        {
            Cursor.lockState = CursorLockMode.None;
            SceneManager.LoadScene(1);
        }
    }
}
