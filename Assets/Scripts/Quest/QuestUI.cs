using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class QuestUI
{
    public string questTitle;
    public List<QuestUIObjective> objectives;
}

[System.Serializable]
public class QuestUIObjective
{
    public string objective;
    public string counter;
    public string additionalText;
}

[System.Serializable]
public enum QuestUIChange
{
    ChangeObjective,
    UpdateCounter,
    UpdateAdditionalText
}