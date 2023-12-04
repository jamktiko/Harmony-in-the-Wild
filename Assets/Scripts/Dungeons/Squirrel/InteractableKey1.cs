using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableKey1 : MonoBehaviour
{
    [SerializeField] bool isActive = false;
    [SerializeField] public bool used = false;
    [SerializeField] int number;
    [SerializeField] AudioSource audioSource;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && isActive && QuestManager.instance.CheckQuestState("SquirrelDungeon"+number).Equals(QuestState.IN_PROGRESS)&&!used|| Input.GetKeyDown(KeyCode.E) && isActive && QuestManager.instance.CheckQuestState("SquirrelDungeon"+number).Equals(QuestState.CAN_START)&&!used)
        {
            used = true;
            if (QuestManager.instance.CheckQuestState("SquirrelDungeon" + number).Equals(QuestState.IN_PROGRESS))
            {
                FindObjectOfType<KeyQuestStep>().CollectableProgress();
            }
            audioSource.Play();
            Debug.Log("object found!");
            gameObject.GetComponent<MeshRenderer>().enabled = false;
            GameObject.Find("Door"+number).SetActive(false);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        isActive = true;
    }
    private void OnTriggerExit(Collider other)
    {
        isActive = false;
    }
}
