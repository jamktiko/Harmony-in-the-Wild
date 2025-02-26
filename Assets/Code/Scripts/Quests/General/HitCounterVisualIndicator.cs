using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class HitCounterVisualIndicator : MonoBehaviour
{
    [FormerlySerializedAs("playerHitCounter")]
    [Header("Needed References")]
    [SerializeField] private HitCounter _playerHitCounter;
    [FormerlySerializedAs("singleHitIndicator")] [SerializeField] private GameObject _singleHitIndicator;

    private int _maxHits;
    private string _currentScene;

    private void Start()
    {
        _currentScene = SceneManager.GetActiveScene().name;

        _maxHits = _playerHitCounter.GetMaxHits();

        for (int i = 0; i < _maxHits; i++)
        {
            Instantiate(_singleHitIndicator, transform);
        }
    }

    private void OnEnable()
    {
        if (_currentScene == "Dungeon_Penguin")
        {
            PenguinRaceManager.instance.PenguinDungeonEvents.OnLapFinished += ResetHealth;
            PenguinRaceManager.instance.PenguinDungeonEvents.OnLapInterrupted += ResetHealth;
        }
    }

    private void OnDisable()
    {
        if (_currentScene == "Dungeon_Penguin")
        {
            PenguinRaceManager.instance.PenguinDungeonEvents.OnLapFinished -= ResetHealth;
            PenguinRaceManager.instance.PenguinDungeonEvents.OnLapInterrupted -= ResetHealth;
        }
    }

    private void ResetHealth()
    {
        foreach (Transform child in transform)
        {
            if (!child.gameObject.activeInHierarchy)
            {
                child.gameObject.SetActive(true);
            }
        }
    }

    public void UpdateHealth(int currentHits)
    {
        if (currentHits != 0)
        {
            transform.GetChild(_maxHits - currentHits).gameObject.SetActive(false);
        }

        else
        {
            ResetHealth();
        }
    }
}
