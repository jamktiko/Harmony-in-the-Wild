using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class TutorialBear : MonoBehaviour
{
    [Header("Quest")]
    [SerializeField] private QuestScriptableObject questInfoForPoint;

    [Header("Dialogue Files")]
    [SerializeField] private List<TextAsset> dialogueFiles;

    private string questId;
    private bool playerIsNear = false;

    private void Awake()
    {
        questId = questInfoForPoint.id;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && playerIsNear)
        {
            InteractWithBear();
        }
    }

    private void InteractWithBear()
    {

    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Trigger"))
        {
            playerIsNear = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Trigger"))
        {
            playerIsNear = false;
        }
    }
}
