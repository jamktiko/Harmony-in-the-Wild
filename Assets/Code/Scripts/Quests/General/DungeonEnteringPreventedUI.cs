using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class DungeonEnteringPreventedUI : MonoBehaviour
{
    [FormerlySerializedAs("requirementText")]
    [Header("Needed References")]
    [SerializeField] private TextMeshProUGUI _requirementText;

    private void Update()
    {
        if (PlayerInputHandler.Instance.CloseUIInput.WasPressedThisFrame())
        {
            CloseView();
        }
    }

    public void SetUIContent(Quest dungeon)
    {
        List<string> prerequisites = new List<string>();
        _requirementText.text = "";

        Debug.Log(dungeon.Info.id + "has " + dungeon.Info.QuestPrerequisites.Length + " quest requirements.");

        foreach (QuestScriptableObject requirement in dungeon.Info.QuestPrerequisites)
        {
            QuestState requirementState = QuestManager.Instance.CheckQuestState(requirement.id);

            if (requirementState != QuestState.Finished)
            {
                prerequisites.Add(requirement.id);
            }
        }

        foreach (string requirement in prerequisites)
        {
            _requirementText.text = _requirementText.text + requirement + "\n \n";
        }
    }

    private void CloseView()
    {
        gameObject.SetActive(false);
    }
}
