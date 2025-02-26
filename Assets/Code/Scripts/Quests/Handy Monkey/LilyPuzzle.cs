using UnityEngine;

public class LilyPuzzle : MonoBehaviour
{
    public static LilyPuzzle instance;

    [HideInInspector] public int socketsFilled = Mathf.Clamp(0, 0, 3);

    private Transform[] lilyTransforms;
    private Vector3[] lilyInitialPositions;
    private int correctLilies = Mathf.Clamp(0, 0, 3);
    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("There is more than one LilyPuzzle object.");
            Destroy(gameObject);
        }
        instance = this;
    }
    void Start()
    {
        //collect The Children.
        int childCount = transform.childCount;
        lilyTransforms = new Transform[childCount];

        for (int i = 0; i < childCount; i++)
        {
            lilyTransforms[i] = transform.GetChild(i).gameObject.transform;
        }

        //store lily positions
        lilyInitialPositions = new Vector3[lilyTransforms.Length];
        for (int i = 0; i < lilyTransforms.Length; i++)
        {
            lilyInitialPositions[i] = lilyTransforms[i].position;
        }
    }
    public void CheckPuzzleProgress(int change)
    {
        correctLilies += change;
        GameEventsManager.instance.questEvents.UpdateQuestProgressInUI("Lilies placed " + correctLilies + "/3");

        if (socketsFilled >= 3)
        {
            if (correctLilies < 3)
            {
                //puzzle failed, reset it
                Invoke("ResetPuzzle", 1f);
                GameEventsManager.instance.questEvents.UpdateQuestProgressInUI("Lilies placed 0/3");
            }
            else
            {
                //complete puzzle, open door
                BossDoorMonkey.instance.CompletePuzzle();
                GameEventsManager.instance.questEvents.ShowQuestUI("The Handy Monkey", "Find the door and complete the quest", "");
            }
        }

        Debug.Log("Progress is: " + correctLilies);
    }
    void ResetPuzzle()
    {
        for (int i = 0; i < lilyTransforms.Length; i++)
        {
            lilyTransforms[i].position = lilyInitialPositions[i];
        }

        //whatever else. particles or audio idk
    }
}
