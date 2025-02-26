using UnityEngine;

public class TreeOfLifeState : MonoBehaviour
{
    public static TreeOfLifeState Instance;

    private int _state = 0;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        else
        {
            Debug.Log("There is more than one Tree Of Life State in the scene.");
        }

        // fetch the latest saved ToL value

        _state = SaveManager.Instance.GetTreeOfLifeState();
    }

    private void OnEnable()
    {
        GameEventsManager.instance.CinematicsEvents.OnStartCinematics += UpdateTreeOfLifeState;
    }

    private void OnDisable()
    {
        GameEventsManager.instance.CinematicsEvents.OnStartCinematics -= UpdateTreeOfLifeState;
    }

    public int GetTreeOfLifeState()
    {
        return _state;
    }

    private void UpdateTreeOfLifeState()
    {
        _state++;

        SaveManager.Instance.SaveGame();
    }
}
