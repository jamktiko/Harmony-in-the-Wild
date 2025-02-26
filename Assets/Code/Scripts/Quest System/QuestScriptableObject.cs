using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/QuestInfo", order = 1)]
public class QuestScriptableObject : ScriptableObject
{
    [field: SerializeField] public string id { get; private set; }
    [FormerlySerializedAs("numericID")] public int NumericID;

    [FormerlySerializedAs("displayName")] [Header("General")]
    public string DisplayName;
    [FormerlySerializedAs("description")] public string Description;
    [FormerlySerializedAs("mainQuest")] public bool MainQuest;
    [FormerlySerializedAs("defaultPos")] public Vector3 DefaultPos;


    [FormerlySerializedAs("levelRequirement")] [Header("Requirements")]
    public int LevelRequirement;
    [FormerlySerializedAs("questPrerequisites")] public QuestScriptableObject[] QuestPrerequisites;

    [FormerlySerializedAs("questStepPrefabs")] [Header("Steps")]
    public GameObject[] QuestStepPrefabs;

    [FormerlySerializedAs("experienceReward")] [Header("Rewards")]
    public int ExperienceReward;
    [FormerlySerializedAs("abilityReward")] public Abilities AbilityReward;

    private void OnValidate()
    {
#if UNITY_EDITOR
        id = this.name;
        UnityEditor.EditorUtility.SetDirty(this);
#endif
    }
}
