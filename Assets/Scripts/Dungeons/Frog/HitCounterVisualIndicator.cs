using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitCounterVisualIndicator : MonoBehaviour
{
    [Header("Needed References")]
    [SerializeField] private HitCounter playerHitCounter;
    [SerializeField] private GameObject singleHitIndicator;

    private int maxHits;
    private int hitsLeft;

    private void Start()
    {
        maxHits = playerHitCounter.maxHits;

        for(int i = 0; i < maxHits; i++)
        {
            Instantiate(singleHitIndicator, transform);
        }

        hitsLeft = maxHits;
    }

    private void OnEnable()
    {
        PenguinRaceManager.instance.penguinDungeonEvents.onLapFinished += ResetHealth;
        PenguinRaceManager.instance.penguinDungeonEvents.onLapInterrupted += ResetHealth;
    }

    private void OnDisable()
    {
        PenguinRaceManager.instance.penguinDungeonEvents.onLapFinished -= ResetHealth;
        PenguinRaceManager.instance.penguinDungeonEvents.onLapInterrupted -= ResetHealth;
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
