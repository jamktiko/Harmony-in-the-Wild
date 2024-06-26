using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BossDoorScript : MonoBehaviour
{
    [SerializeField] private List<LeverScript> scripts=new List<LeverScript>();
    public bool isOpen;
    public int usedlevers, levers;
    [SerializeField] Material usedMat;

    public Animator anim;

    void Start()
    {
        scripts=FindObjectsOfType<LeverScript>().ToList();
        usedlevers=scripts.Where(x=>x.wasUsed).Count();
        levers= scripts.Where(x => !x.wasUsed).Count();
    }

    void Update()
    {
        if (usedlevers == levers&&isOpen==false) 
        {
            isOpen = true;
            gameObject.GetComponent<AudioSource>().Play();
            anim.Play("Door_Open_ANI");
            //GameEventsManager.instance.questEvents.UpdateQuestUI(QuestUIChange.ChangeObjective, "");
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (isOpen&&other.CompareTag("Trigger")) 
        {
            SceneManager.LoadScene("Dungeon_Squirrel_Boss");
        }
    }

    public void UpdateQuestUI()
    {
        //GameEventsManager.instance.questEvents.UpdateQuestUI(QuestUIChange.UpdateCounter, "Levers activated " + usedlevers + "/" + levers);
    }
}
