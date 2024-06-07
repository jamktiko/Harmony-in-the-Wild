using UnityEngine;

public class LeverScript : MonoBehaviour
{
    [SerializeField] bool isActive = false;
    [SerializeField] public bool wasUsed = false; //TODO: Check usecase and fix this being public.
    [SerializeField] Material usedMat;
    [SerializeField] string questName;
    [SerializeField] static int stageIndex;
    private BossDoorScript bossDoorScript;

    public Animator anim; //NOTE: Why is this public?

    private void Start()
    {
        bossDoorScript = FindObjectOfType<BossDoorScript>();
        stageIndex = bossDoorScript.usedlevers;
    }

    void Update()
    {
        if (PlayerInputHandler.instance.InteractInput.WasPressedThisFrame() && isActive&&!wasUsed)
        {
            wasUsed = true;
            Debug.Log("Lever pulled!");
            bossDoorScript.usedlevers++;
            stageIndex++;
            bossDoorScript.UpdateQuestUI();
            //gameObject.GetComponent<MeshRenderer>().material= usedMat;
            gameObject.GetComponent<AudioSource>().Play();
            anim.Play("Leaver_Turn_ANI");
            GameEventsManager.instance.questEvents.AdvanceDungeonQuest(questName, stageIndex);
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
