using UnityEngine;
using UnityEngine.SceneManagement;

public class TooSlowView : MonoBehaviour
{
    private void Start()
    {
        PenguinRaceManager.instance.PenguinDungeonEvents.OnTimeRanOut += ShowTheView;
    }

    private void OnDisable()
    {
        PenguinRaceManager.instance.PenguinDungeonEvents.OnTimeRanOut -= ShowTheView;
    }

    private void ShowTheView()
    {
        transform.GetChild(0).gameObject.SetActive(true);
    }

    public void TryAgain()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
