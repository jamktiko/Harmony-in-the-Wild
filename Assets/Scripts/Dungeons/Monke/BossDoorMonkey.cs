using UnityEngine;

public class BossDoorMonkey : MonoBehaviour
{
    public static BossDoorMonkey instance;

    [SerializeField] private Animator anim;

    private int shapesInCorrectPlaces = 0;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("There is more than one BossDoorMonkey object.");
            Destroy(gameObject);
        }
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
