using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

// enables transition to the ending storybook

public class EndingTransition : MonoBehaviour
{
    public const string StorybookSceneName = "Storybook";

    [FormerlySerializedAs("questSO")] [SerializeField] private QuestScriptableObject _questSo;
    [FormerlySerializedAs("storybookSectionIndex")] [SerializeField] private int _storybookSectionIndex;
    [FormerlySerializedAs("goToScene")] [SerializeField] private SceneManagerHelper.Scene _goToScene;

    private void Start()
    {
        StartCoroutine(QuestProgressCheckDelay());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Trigger") && QuestManager.Instance.CheckQuestState(_questSo.id) == QuestState.CanStart)
        {
            StorybookHandler.Instance.SetNewStorybookData(_storybookSectionIndex, _goToScene, false);
            SceneManager.LoadScene(StorybookSceneName);
        }
    }

    // NOTE! CHANGE THIS TO START ONCE THE MANAGERS ARE INITIALIZED IN MAIN MENU
    // NOTE! THIS DELAY IS HERE FOR TESTING PURPOSES ONLY TO MAKE SURE THE SAVED DATA IS LOADED BEFORE TRYING TO REFERENCE TO IT

    private IEnumerator QuestProgressCheckDelay()
    {
        yield return new WaitForSeconds(1);

        if (QuestManager.Instance.CheckQuestState(_questSo.id) != QuestState.CanStart)
        {
            gameObject.SetActive(false);
        }
    }
}
