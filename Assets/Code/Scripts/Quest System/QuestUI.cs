using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;

public class QuestUI : MonoBehaviour
{
    [Header("Needed References")]
    [SerializeField] private List<TextMeshProUGUI> questTextComponents;

    //public UnityEvent AddTextToUI=new UnityEvent();
    private void OnEnable()
    {
        GameEventsManager.instance.questEvents.OnShowQuestUI += InitializeQuestUI;
        GameEventsManager.instance.questEvents.OnUpdateQuestProgressInUI += ShowQuestProgressInUI;
        GameEventsManager.instance.questEvents.OnHideQuestUI += HideQuestUI;
    }

    private void OnDisable()
    {
        GameEventsManager.instance.questEvents.OnShowQuestUI -= InitializeQuestUI;
        GameEventsManager.instance.questEvents.OnUpdateQuestProgressInUI -= ShowQuestProgressInUI;
        GameEventsManager.instance.questEvents.OnHideQuestUI -= HideQuestUI;
    }

    private void InitializeQuestUI(string questName, string description, string progress)
    {
        transform.GetChild(0).gameObject.SetActive(true);

        questTextComponents[0].text = questName;
        questTextComponents[1].text = description;
        questTextComponents[2].text = progress;
    }

    private void ShowQuestProgressInUI(string currentState)
    {
        questTextComponents[2].text = currentState;
    }

    private void HideQuestUI()
    {
        transform.GetChild(0).gameObject.SetActive(false);
    }
}
