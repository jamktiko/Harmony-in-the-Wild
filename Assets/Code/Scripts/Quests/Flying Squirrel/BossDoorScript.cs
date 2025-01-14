using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BossDoorScript : MonoBehaviour
{
    [SerializeField] private List<LeverScript> scripts=new List<LeverScript>();
    public bool isOpen;
    public int usedlevers, levers;
    [SerializeField] Material usedMat;

    public Animator anim;
    private FinishDungeonQuestStepWithTrigger finishDungeonQuestStep;

    void Start()
    {
        scripts=FindObjectsOfType<LeverScript>().ToList();
        usedlevers=scripts.Where(x=>x.wasUsed).Count();
        levers= scripts.Where(x => !x.wasUsed).Count();

        finishDungeonQuestStep = GetComponent<FinishDungeonQuestStepWithTrigger>();
    }

    void Update()
    {
        if (usedlevers == levers&&isOpen==false) 
        {
            isOpen = true;
            gameObject.GetComponent<AudioSource>().Play();
            anim.Play("Door_Open_ANI");

            finishDungeonQuestStep.EnableInteraction();
            GameEventsManager.instance.questEvents.ShowQuestUI("The Flying Squirrel", "Find the door and enter to the boss area", "");
        }
    }

    public void UpdateQuestUI()
    {
        GameEventsManager.instance.questEvents.UpdateQuestProgressInUI("Levers activated " + usedlevers + "/" + levers);
    }
}
