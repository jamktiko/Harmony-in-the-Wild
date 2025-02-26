using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class QuestUI : MonoBehaviour
{
    [Header("Needed References")]
    [SerializeField] public List<TextMeshProUGUI> questTextComponents;
    [SerializeField] private QuestWaypoint questWaypoint;

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

        questTextComponents[0].text = ProcessTextForInputs(questName);
        questTextComponents[1].text = ProcessTextForInputs(description);
        questTextComponents[2].text = ProcessTextForInputs(progress);
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
                if (index - 1 > i && InputSprites.instance.keySetups.ContainsKey(text.Substring(i + 1, index - i - 1)))
                {
                    if (Gamepad.current == null || Keyboard.current.lastUpdateTime > Gamepad.current.lastUpdateTime || Mouse.current.lastUpdateTime > Gamepad.current.lastUpdateTime)
                        ret += InputSprites.instance.keySetups[text.Substring(i + 1, index - i - 1)].keyboard;
                    else
                        ret += InputSprites.instance.keySetups[text.Substring(i + 1, index - i - 1)].gamepad;
                }
                index++; i = index;
            }
        }

        if (ret.Length < 1) return text;
        return ret;
    }

    private void ShowQuestProgressInUI(string currentState)
    {
        questTextComponents[2].text = currentState;
    }

    private void HideQuestUI()
    {
        transform.GetChild(0).gameObject.SetActive(false);
    }

    public string getCurrentQuestName()
    {
        return questTextComponents[0].text.ToString();
    }
}
