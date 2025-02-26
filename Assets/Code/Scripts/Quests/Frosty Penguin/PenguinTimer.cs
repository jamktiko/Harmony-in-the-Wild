using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class PenguinTimer : MonoBehaviour
{
    [FormerlySerializedAs("timerText")] [SerializeField] private TextMeshProUGUI _timerText;
    [FormerlySerializedAs("maxTime")] [SerializeField] private float _maxTime = 180f;

    private bool _raceInProgress;
    private float _currentTime;

    private void OnEnable()
    {
        PenguinRaceManager.instance.PenguinDungeonEvents.OnRaceFinished += StopTimer;
    }

    private void OnDisable()
    {
        PenguinRaceManager.instance.PenguinDungeonEvents.OnRaceFinished -= StopTimer;
    }

    private void Update()
    {
        if (_raceInProgress)
        {
            _currentTime += Time.deltaTime;

            if (_currentTime >= _maxTime)
            {
                _raceInProgress = false;
                PenguinRaceManager.instance.PenguinDungeonEvents.TimeRanOut();
            }

            UpdateTimer();
        }
    }

    public void ToggleTimer(bool timerOn)
    {
        transform.GetChild(0).gameObject.SetActive(timerOn);
        _raceInProgress = timerOn;
    }

    private void StopTimer()
    {
        ToggleTimer(false);
    }

    private void UpdateTimer()
    {
        int minutes = Mathf.FloorToInt(_currentTime / 60);
        int seconds = Mathf.FloorToInt(_currentTime % 60);
        int milliseconds = Mathf.FloorToInt(_currentTime * 100 % 100);

        _timerText.text = string.Format("{0:00}:{1:00}:{2:00}", minutes, seconds, milliseconds);
    }

    public string GetFinalTimeAsString()
    {
        int minutes = Mathf.FloorToInt(_currentTime / 60);
        int seconds = Mathf.FloorToInt(_currentTime % 60);
        int milliseconds = Mathf.FloorToInt(_currentTime * 100 % 100);

        return string.Format("{0:00}:{1:00}:{2:00}", minutes, seconds, milliseconds);
    }

    public float GetFinalTimeAsFloat()
    {
        return _currentTime;
    }

    private void UpdateLapCounter()
    {

    }
}