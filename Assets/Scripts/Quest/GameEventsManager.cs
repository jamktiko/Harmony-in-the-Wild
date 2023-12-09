using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEventsManager : MonoBehaviour
{
    public static GameEventsManager instance { get; private set; }

    public QuestEvents questEvents;
    public PlayerEvents playerEvents;

    private void Awake()
    {
        // create singleton
        if(instance != null)
        {
            Debug.LogError("There is more than one Game Events Manager in the scene");
            Destroy(gameObject);
        }

        instance = this;

        // initialize the events
        questEvents = new QuestEvents();
        playerEvents = new PlayerEvents();
    }
}
