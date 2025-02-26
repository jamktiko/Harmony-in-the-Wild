using UnityEngine;

public class GameEventsManager : MonoBehaviour
{
    public static GameEventsManager instance { get; private set; }

    public QuestEvents QuestEvents;
    public PlayerEvents PlayerEvents;
    public DialogueEvents DialogueEvents;
    public CinematicsEvents CinematicsEvents;
    public UIEvents UIEvents;
    public AudioEvents AudioEvents;

    private void Awake()
    {
        if (GameEventsManager.instance != null)
        {
            Debug.LogWarning("There is more than one Game Events Manager in the scene");
            Destroy(gameObject);
        }

        else
        {
            instance = this;

        }

        // initialize the events
        QuestEvents = new QuestEvents();
        PlayerEvents = new PlayerEvents();
        DialogueEvents = new DialogueEvents();
        CinematicsEvents = new CinematicsEvents();
        UIEvents = new UIEvents();
        AudioEvents = new AudioEvents();
    }
}
