using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableBear : MonoBehaviour
{
    [SerializeField] private QuestScriptableObject tutorialQuest;

    private void Start()
    {
        Transform questManager = QuestManager.instance.transform;

        foreach(Transform child in questManager)
        {
            if (child.name.Contains("Bear"))
            {
                gameObject.SetActive(false);
            }
        }
    }
}
