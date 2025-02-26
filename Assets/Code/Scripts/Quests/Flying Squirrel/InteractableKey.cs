using UnityEngine;
using UnityEngine.Serialization;

public class InteractableKey : MonoBehaviour
{
    [FormerlySerializedAs("isActive")] [SerializeField] bool _isActive = false;
    [FormerlySerializedAs("wasUsed")] [SerializeField] public bool WasUsed = false; //TODO: check what usecase is and fix accordingly
    [FormerlySerializedAs("doorToOpen")] [SerializeField] private GameObject _doorToOpen;
    //[SerializeField] int keyNumber;
    [FormerlySerializedAs("keySoundAudioSource")] [SerializeField] AudioSource _keySoundAudioSource;
    [FormerlySerializedAs("keyCounter")] [SerializeField] private SquirrelBossKeyCounter _keyCounter;

    void Update()
    {
        if (PlayerInputHandler.Instance.InteractInput.WasPressedThisFrame() && _isActive && !WasUsed)
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
        WasUsed = true;
        _keySoundAudioSource.Play();
        gameObject.GetComponentInChildren<MeshRenderer>().enabled = false;
        _doorToOpen.SetActive(false);
        _keyCounter.CollectKey();
    }

    private void OnTriggerEnter(Collider other)
    {
        _isActive = true;
    }

    private void OnTriggerExit(Collider other)
    {
        _isActive = false;
    }
}
