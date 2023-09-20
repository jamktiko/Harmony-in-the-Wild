using System.Collections;
using System.Collections.Generic;
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
    public GameObject[] QuestStepPrefabs;
    [Header("Rewards")]
    public int ExperienceReward;
    // Start is called before the first frame update
    private void OnValidate()
    {
#if UNITY_EDITOR
        id = this.name;
        UnityEditor.EditorUtility.SetDirty(this);
#endif
    }
}
