using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndingTransition : MonoBehaviour
{
    public const string StorybookSceneName = "Storybook";

    [SerializeField] private QuestScriptableObject questSO;
    [SerializeField] private int storybookSectionIndex;
    [SerializeField] private string goToScene;

    private void Start()
    {
        StartCoroutine(QuestProgressCheckDelay());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Trigger") && QuestManager.instance.CheckQuestState(questSO.id) == QuestState.CAN_START)
        {
            StorybookHandler.instance.SetNewStorybookData(storybookSectionIndex, goToScene, false);
            SceneManager.LoadScene(StorybookSceneName);
        }
    }

    // NOTE! CHANGE THIS TO START ONCE THE MANAGERS ARE INITIALIZED IN MAIN MENU
    // NOTE! THIS DELAY IS HERE FOR TESTING PURPOSES ONLY TO MAKE SURE THE SAVED DATA IS LOADED BEFORE TRYING TO REFERENCE TO IT

    private IEnumerator QuestProgressCheckDelay()
    {
        yield return new WaitForSeconds(1);

        if (QuestManager.instance.CheckQuestState(questSO.id) != QuestState.CAN_START)
        {
            gameObject.SetActive(false);
        }
    }
}
