using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDoorMonkey : MonoBehaviour
{
    private int shapesInCorrectPlaces = 0;

    public static BossDoorMonkey instance;

    [SerializeField] private Animator anim;

    private void Awake()
    {
        instance = this;
    }

    public void UpdateProgress(int change)
    {
        shapesInCorrectPlaces += change;

        if(shapesInCorrectPlaces >= 4)
        {
            gameObject.GetComponent<AudioSource>().Play();
            anim.Play("Door_Open_ANI");
            GameEventsManager.instance.questEvents.UpdateQuestUI(QuestUIChange.ChangeObjective, "");
        }
    }
}
