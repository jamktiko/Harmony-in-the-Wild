using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DungeonEnteringPreventedUI : MonoBehaviour
{
    [Header("Needed References")]
    [SerializeField] private TextMeshProUGUI requirementText;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            CloseView();
        }
    }

    public void SetUIContent(Quest dungeon)
    {
        List<string> prerequisites = new List<string>();
        requirementText.text = "";

        Debug.Log(dungeon.info.id + "has " + dungeon.info.questPrerequisites.Length + " quest requirements.");

        foreach(QuestScriptableObject requirement in dungeon.info.questPrerequisites)
        {
            QuestState requirementState = QuestManager.instance.CheckQuestState(requirement.id);

            if(requirementState != QuestState.FINISHED)
            {
                prerequisites.Add(requirement.id);
            }
        }

        foreach(string requirement in prerequisites)
        {
            requirementText.text = requirementText.text + requirement + "\n \n";
        }
    }

    private void CloseView()
    {
        gameObject.SetActive(false);
    }
}
