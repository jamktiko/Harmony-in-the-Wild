using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShowQuestUI : MonoBehaviour
{
    [Header("Needed References")]
    [SerializeField] private List<TextMeshProUGUI> questTextComponents;

    [Header("Quest Texts")]
    [SerializeField] private List<QuestUI> questTextContents;

    private QuestUI currentQuestUI;
    private int currentObjective;

    private void OnEnable()
    {
        GameEventsManager.instance.questEvents.OnShowQuestUI += InitializeQuestUI;
        GameEventsManager.instance.questEvents.OnUpdateQuestUI += UpdateQuestUI;
        GameEventsManager.instance.questEvents.OnHideQuestUI += HideQuestUI;
    }

    private void OnDisable()
    {
        GameEventsManager.instance.questEvents.OnShowQuestUI -= InitializeQuestUI;
        GameEventsManager.instance.questEvents.OnUpdateQuestUI -= UpdateQuestUI;
        GameEventsManager.instance.questEvents.OnHideQuestUI -= HideQuestUI;
    }

    private void InitializeQuestUI(int questTextIndex)
    {
        Debug.Log("Show quest UI.");

        transform.GetChild(0).gameObject.SetActive(true);

        currentQuestUI = questTextContents[questTextIndex];

        for(int i = 0; i < questTextComponents.Count; i++)
        {
            switch (i)
            {
                case 0:
                    questTextComponents[i].text = currentQuestUI.questTitle;
                    break;

                case 1:
                    questTextComponents[i].text = currentQuestUI.objectives[0].objective;
                    break;

                case 2:
                    if(currentQuestUI.objectives[0].counter != "")
                    {
                        questTextComponents[i].gameObject.SetActive(true);
                        questTextComponents[i].text = currentQuestUI.objectives[0].counter;
                    }

                    else
                    {
                        questTextComponents[i].gameObject.SetActive(false);
                    }
                    break;

                case 3:
                    /*if (currentQuestUI.objectives[0].counter != "")
                    {
                        questTextComponents[i].gameObject.SetActive(true);
                        questTextComponents[i].text = currentQuestUI.objectives[0].additionalText;
                    }

                    else
                    {
                        questTextComponents[i].gameObject.SetActive(false);
                    }*/
                    break;

                default:
                    Debug.Log("Error in setting Quest UI!");
                    break;
            }
        }
    }

    private void UpdateQuestUI(QuestUIChange changeType, string newText)
    {
        switch (changeType)
        {
            case QuestUIChange.ChangeObjective:
                questTextComponents[1].text = currentQuestUI.objectives[1].objective;

                questTextComponents[2].gameObject.SetActive(false);
                questTextComponents[3].gameObject.SetActive(false);
                break;

            case QuestUIChange.UpdateCounter:
                questTextComponents[2].text = newText;
                break;

            case QuestUIChange.UpdateAdditionalText:
                questTextComponents[3].text = newText;
                break;

            default:
                Debug.LogError("Error while updating Quest UI");
                break;
        }
    }

    private void HideQuestUI()
    {
        transform.GetChild(0).gameObject.SetActive(false);
    }
}
