using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

public class BossDoorScript : MonoBehaviour
{
    [FormerlySerializedAs("scripts")] [SerializeField] private List<LeverScript> _scripts = new List<LeverScript>();
    [FormerlySerializedAs("isOpen")] public bool IsOpen;
    [FormerlySerializedAs("usedlevers")] public int Usedlevers;
    [FormerlySerializedAs("levers")] public int Levers;
    [FormerlySerializedAs("usedMat")] [SerializeField]
    private Material _usedMat;

    [FormerlySerializedAs("anim")] public Animator Anim;
    private FinishDungeonQuestStepWithTrigger _finishDungeonQuestStep;

    private void Start()
    {
        _scripts = FindObjectsOfType<LeverScript>().ToList();
        Usedlevers = _scripts.Where(x => x.WasUsed).Count();
        Levers = _scripts.Where(x => !x.WasUsed).Count();

        _finishDungeonQuestStep = GetComponent<FinishDungeonQuestStepWithTrigger>();
    }

    private void Update()
    {
        if (Usedlevers == Levers && IsOpen == false)
        {
            IsOpen = true;
            gameObject.GetComponent<AudioSource>().Play();
            Anim.Play("Door_Open_ANI");

            _finishDungeonQuestStep.EnableInteraction();
            GameEventsManager.instance.QuestEvents.ShowQuestUI("The Flying Squirrel", "Find the door and enter to the boss area", "");
        }
    }

    public void UpdateQuestUI()
    {
        GameEventsManager.instance.QuestEvents.UpdateQuestProgressInUI("Levers activated " + Usedlevers + "/" + Levers);
    }
}
