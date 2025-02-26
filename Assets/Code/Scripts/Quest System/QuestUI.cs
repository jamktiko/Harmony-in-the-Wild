using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class QuestUI : MonoBehaviour
{
    [FormerlySerializedAs("questTextComponents")]
    [Header("Needed References")]
    [SerializeField] public List<TextMeshProUGUI> QuestTextComponents;
    [FormerlySerializedAs("questWaypoint")] [SerializeField] private QuestWaypoint _questWaypoint;

    //public UnityEvent AddTextToUI=new UnityEvent();
    private void OnEnable()
    {
        GameEventsManager.instance.QuestEvents.OnShowQuestUI += InitializeQuestUI;
        GameEventsManager.instance.QuestEvents.OnUpdateQuestProgressInUI += ShowQuestProgressInUI;
        GameEventsManager.instance.QuestEvents.OnHideQuestUI += HideQuestUI;
    }

    private void OnDisable()
    {
        GameEventsManager.instance.QuestEvents.OnShowQuestUI -= InitializeQuestUI;
        GameEventsManager.instance.QuestEvents.OnUpdateQuestProgressInUI -= ShowQuestProgressInUI;
        GameEventsManager.instance.QuestEvents.OnHideQuestUI -= HideQuestUI;
    }

    private void InitializeQuestUI(string questName, string description, string progress)
    {
        transform.GetChild(0).gameObject.SetActive(true);

        QuestTextComponents[0].text = ProcessTextForInputs(questName);
        QuestTextComponents[1].text = ProcessTextForInputs(description);
        QuestTextComponents[2].text = ProcessTextForInputs(progress);
    }

    // Detect inputs from string denoted as |(input)|, and change them to the appropriate input method key.
    private string ProcessTextForInputs(string text)
    {
        string ret = "";
        int index = 0;
        for (int i = 0; i < text.Length; i++)
        {
            if (text[i] == '|')
            {
                ret += text.Substring(index, i - index);
                index = i + 1;
                while (text[index] != '|')
                    index++;
                if (index - 1 > i && InputSprites.Instance.KeySetups.ContainsKey(text.Substring(i + 1, index - i - 1)))
                {
                    if (Gamepad.current == null || Keyboard.current.lastUpdateTime > Gamepad.current.lastUpdateTime || Mouse.current.lastUpdateTime > Gamepad.current.lastUpdateTime)
                        ret += InputSprites.Instance.KeySetups[text.Substring(i + 1, index - i - 1)].Keyboard;
                    else
                        ret += InputSprites.Instance.KeySetups[text.Substring(i + 1, index - i - 1)].Gamepad;
                }
                index++; i = index;
            }
        }

        if (ret.Length < 1) return text;
        return ret;
    }

    private void ShowQuestProgressInUI(string currentState)
    {
        QuestTextComponents[2].text = currentState;
    }

    private void HideQuestUI()
    {
        transform.GetChild(0).gameObject.SetActive(false);
    }

    public string GetCurrentQuestName()
    {
        return QuestTextComponents[0].text.ToString();
    }
}
