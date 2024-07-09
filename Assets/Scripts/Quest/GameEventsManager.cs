using UnityEngine;

public class GameEventsManager : MonoBehaviour
{
    public static GameEventsManager instance { get; private set; }

    public QuestEvents questEvents;
    public PlayerEvents playerEvents;
    public DialogueEvents dialogueEvents;
    public CinematicsEvents cinematicsEvents;

    private void Awake()
    {
        if(DontDestroyOnLoadManagers.instance != null)
        {
            Debug.LogWarning("There is more than one Game Events Manager in the scene");
            Destroy(gameObject);
        }

        else
        {
            instance = this;

        }

        // initialize the events
        questEvents = new QuestEvents();
        playerEvents = new PlayerEvents();
        dialogueEvents = new DialogueEvents();
        cinematicsEvents = new CinematicsEvents();
    }
}
