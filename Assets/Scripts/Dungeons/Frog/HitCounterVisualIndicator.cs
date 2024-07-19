using UnityEngine;
using UnityEngine.SceneManagement;

public class HitCounterVisualIndicator : MonoBehaviour
{
    [Header("Needed References")]
    [SerializeField] private HitCounter playerHitCounter;
    [SerializeField] private GameObject singleHitIndicator;

    private int maxHits;
    private string currentScene;

    private void Start()
    {
        currentScene = SceneManager.GetActiveScene().name;

        maxHits = playerHitCounter.GetMaxHits();
        
        for(int i = 0; i < maxHits; i++)
        {
            Instantiate(singleHitIndicator, transform);
        }
    }

    private void OnEnable()
    {
        if(currentScene == "RallyBuild_Penguin")
        {
            PenguinRaceManager.instance.penguinDungeonEvents.onLapFinished += ResetHealth;
            PenguinRaceManager.instance.penguinDungeonEvents.onLapInterrupted += ResetHealth;
        }
    }

    private void OnDisable()
    {
        if(currentScene == "RallyBuild_Penguin")
        {
            PenguinRaceManager.instance.penguinDungeonEvents.onLapFinished -= ResetHealth;
            PenguinRaceManager.instance.penguinDungeonEvents.onLapInterrupted -= ResetHealth;
        }
    }

    private void ResetHealth()
    {
        foreach(Transform child in transform)
        {
            if (!child.gameObject.activeInHierarchy)
            {
                child.gameObject.SetActive(true);
            }
        }
    }

    public void UpdateHealth(int currentHits)
    {
        if(currentHits != 0)
        {
            transform.GetChild(maxHits - currentHits).gameObject.SetActive(false);
        }

        else
        {
            ResetHealth();
        }
    }
}
