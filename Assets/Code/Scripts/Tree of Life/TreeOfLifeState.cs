using UnityEngine;

public class TreeOfLifeState : MonoBehaviour
{
    public static TreeOfLifeState instance;

    private int state = 0;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        else
        {
            Debug.Log("There is more than one Tree Of Life State in the scene.");
        }

        // fetch the latest saved ToL value

        state = SaveManager.instance.GetTreeOfLifeState();
    }

    private void OnEnable()
    {
        GameEventsManager.instance.cinematicsEvents.OnStartCinematics += UpdateTreeOfLifeState;
    }

    private void OnDisable()
    {
        GameEventsManager.instance.cinematicsEvents.OnStartCinematics -= UpdateTreeOfLifeState;
    }

    public int GetTreeOfLifeState()
    {
        return state;
    }

    private void UpdateTreeOfLifeState()
    {
        state++;

        SaveManager.instance.SaveGame();
    }
}
