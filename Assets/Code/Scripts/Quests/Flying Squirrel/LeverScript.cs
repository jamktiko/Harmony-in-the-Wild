using UnityEngine;
using UnityEngine.Serialization;

public class LeverScript : MonoBehaviour
{
    [FormerlySerializedAs("isActive")] [SerializeField] bool _isActive = false;
    [FormerlySerializedAs("wasUsed")] [SerializeField] public bool WasUsed = false; //TODO: Check usecase and fix this being public.
    [FormerlySerializedAs("usedMat")] [SerializeField] Material _usedMat;
    [FormerlySerializedAs("questName")] [SerializeField] string _questName;
    private BossDoorScript _bossDoorScript;

    [FormerlySerializedAs("anim")] public Animator Anim; //NOTE: Why is this public?

    private void Start()
    {
        _bossDoorScript = FindObjectOfType<BossDoorScript>();
    }

    void Update()
    {
        if (PlayerInputHandler.Instance.InteractInput.WasPressedThisFrame() && _isActive && !WasUsed)
        {
            WasUsed = true;
            Debug.Log("Lever pulled!");
            _bossDoorScript.Usedlevers++;
            _bossDoorScript.UpdateQuestUI();
            //gameObject.GetComponent<MeshRenderer>().material= usedMat;
            gameObject.GetComponent<AudioSource>().Play();
            Anim.Play("Leaver_Turn_ANI");
        }
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
