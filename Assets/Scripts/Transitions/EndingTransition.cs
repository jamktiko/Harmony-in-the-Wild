using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndingTransition : MonoBehaviour
{
    [SerializeField] private QuestScriptableObject questSO;
    [SerializeField] private int storybookSectionIndex;
    [SerializeField] private string goToScene;

    private void Start()
    {
        if(QuestManager.instance.CheckQuestState(questSO.id) != QuestState.CAN_START)
        {
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Trigger") && QuestManager.instance.CheckQuestState(questSO.id) == QuestState.CAN_START)
        {
            StorybookHandler.instance.SetNewStorybookData(storybookSectionIndex, goToScene, false);
            SceneManager.LoadScene("Storybook");
        }
    }
}
