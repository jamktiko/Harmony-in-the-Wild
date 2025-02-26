using UnityEngine;

public class InteractableKey : MonoBehaviour
{
    [SerializeField] bool isActive = false;
    [SerializeField] public bool wasUsed = false; //TODO: check what usecase is and fix accordingly
    [SerializeField] private GameObject doorToOpen;
    //[SerializeField] int keyNumber;
    [SerializeField] AudioSource keySoundAudioSource;
    [SerializeField] private SquirrelBossKeyCounter keyCounter;

    void Update()
    {
        if (PlayerInputHandler.instance.InteractInput.WasPressedThisFrame() && isActive && !wasUsed)
        {
            UseKey();
        }

        /*if (Input.GetKeyDown(KeyCode.E) && isActive && QuestManager.instance.CheckQuestState("The Flying Squirrel").Equals(QuestState.IN_PROGRESS)&&!wasUsed|| Input.GetKeyDown(KeyCode.E) && isActive && QuestManager.instance.CheckQuestState("SquirrelDungeon"+keyNumber).Equals(QuestState.CAN_START)&&!wasUsed)
        {
            wasUsed = true;
            if (QuestManager.instance.CheckQuestState("The Flying Squirrel").Equals(QuestState.IN_PROGRESS))
            {
                FindObjectOfType<KeyQuestStep>().CollectableProgress();
            }
            keySoundAudioSource.Play();
            Debug.Log("object found!");
            gameObject.GetComponentInChildren<MeshRenderer>().enabled = false;
            GameObject.Find("Door"+keyNumber).SetActive(false);
        }*/
    }

    private void UseKey()
    {
        wasUsed = true;
        keySoundAudioSource.Play();
        gameObject.GetComponentInChildren<MeshRenderer>().enabled = false;
        doorToOpen.SetActive(false);
        keyCounter.CollectKey();
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
