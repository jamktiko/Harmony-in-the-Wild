using System;
using UnityEngine;
using UnityEngine.Serialization;

[Serializable]
public class NpcQuestDialoguePair
{
    [FormerlySerializedAs("mainQuest")] public QuestScriptableObject MainQuest;
    [FormerlySerializedAs("dialogueOption")] public TextAsset DialogueOption;
}
