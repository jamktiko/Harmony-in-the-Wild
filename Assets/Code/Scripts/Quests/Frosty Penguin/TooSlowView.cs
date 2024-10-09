using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TooSlowView : MonoBehaviour
{
    private void Start()
    {
        PenguinRaceManager.instance.penguinDungeonEvents.onTimeRanOut += ShowTheView;
    }

    private void OnDisable()
    {
        PenguinRaceManager.instance.penguinDungeonEvents.onTimeRanOut -= ShowTheView;
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
