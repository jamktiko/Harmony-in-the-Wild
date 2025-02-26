using UnityEngine;
using UnityEngine.Serialization;

public class LilyPuzzle : MonoBehaviour
{
    public static LilyPuzzle Instance;

    [FormerlySerializedAs("socketsFilled")] [HideInInspector] public int SocketsFilled = Mathf.Clamp(0, 0, 3);

    private Transform[] _lilyTransforms;
    private Vector3[] _lilyInitialPositions;
    private int _correctLilies = Mathf.Clamp(0, 0, 3);
    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogWarning("There is more than one LilyPuzzle object.");
            Destroy(gameObject);
        }
        Instance = this;
    }
    void Start()
    {
        //collect The Children.
        int childCount = transform.childCount;
        _lilyTransforms = new Transform[childCount];

        for (int i = 0; i < childCount; i++)
        {
            _lilyTransforms[i] = transform.GetChild(i).gameObject.transform;
        }

        //store lily positions
        _lilyInitialPositions = new Vector3[_lilyTransforms.Length];
        for (int i = 0; i < _lilyTransforms.Length; i++)
        {
            _lilyInitialPositions[i] = _lilyTransforms[i].position;
        }
    }
    public void CheckPuzzleProgress(int change)
    {
        _correctLilies += change;
        GameEventsManager.instance.QuestEvents.UpdateQuestProgressInUI("Lilies placed " + _correctLilies + "/3");

        if (SocketsFilled >= 3)
        {
            if (_correctLilies < 3)
            {
                //puzzle failed, reset it
                Invoke("ResetPuzzle", 1f);
                GameEventsManager.instance.QuestEvents.UpdateQuestProgressInUI("Lilies placed 0/3");
            }
            else
            {
                //complete puzzle, open door
                BossDoorMonkey.Instance.CompletePuzzle();
                GameEventsManager.instance.QuestEvents.ShowQuestUI("The Handy Monkey", "Find the door and complete the quest", "");
            }
        }

        Debug.Log("Progress is: " + _correctLilies);
    }
    void ResetPuzzle()
    {
        for (int i = 0; i < _lilyTransforms.Length; i++)
        {
            _lilyTransforms[i].position = _lilyInitialPositions[i];
        }

        //whatever else. particles or audio idk
    }
}
