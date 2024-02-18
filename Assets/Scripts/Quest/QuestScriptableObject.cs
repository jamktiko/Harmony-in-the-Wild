using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/QuestInfo", order = 1)]
public class QuestScriptableObject : ScriptableObject
{
    
    [field: SerializeField] public string id { get; private set; }

    [Header("General")]
    public string displayName;

    [Header("Requirements")]
    public int levelRequirement;
    public QuestScriptableObject[] questPrerequisites;
    
    [Header("Steps")]
    public GameObject[] questStepPrefabs;
    
    [Header("Rewards")]
    public int experienceReward;
    public int abilityReward;

    private void OnValidate()
    {
#if UNITY_EDITOR
        id = this.name;
        UnityEditor.EditorUtility.SetDirty(this);
#endif
    }
}
