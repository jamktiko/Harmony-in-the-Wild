using UnityEngine;

public class LeverScript : MonoBehaviour
{
    [SerializeField] bool isActive = false;
    [SerializeField] public bool wasUsed = false; //TODO: Check usecase and fix this being public.
    [SerializeField] Material usedMat;

    private BossDoorScript bossDoorScript;

    public Animator anim; //NOTE: Why is this public?

    private void Start()
    {
        bossDoorScript = FindObjectOfType<BossDoorScript>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && isActive&&!wasUsed)
        {
            wasUsed = true;
            Debug.Log("Lever pulled!");
            bossDoorScript.usedlevers++;
            bossDoorScript.UpdateQuestUI();
            //gameObject.GetComponent<MeshRenderer>().material= usedMat;
            gameObject.GetComponent<AudioSource>().Play();
            anim.Play("Leaver_Turn_ANI");
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
