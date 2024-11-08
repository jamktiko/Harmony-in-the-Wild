using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/QuestInfo", order = 1)]
public class QuestScriptableObject : ScriptableObject
{
    [field: SerializeField] public string id { get; private set; }
    public int numericID;

    [Header("General")]
    public string displayName;
    public string description;
    public bool mainQuest;

    [Header("Requirements")]
    public int levelRequirement;
    public QuestScriptableObject[] questPrerequisites;
    
    [Header("Steps")]
    public GameObject[] questStepPrefabs;
    
    [Header("Rewards")]
    public int experienceReward;
    public Abilities abilityReward;

    private void OnValidate()
    {
#if UNITY_EDITOR
        id = this.name;
        numericID = QuestManager.instance.NextID();
        UnityEditor.EditorUtility.SetDirty(this);
#endif
    }
}
