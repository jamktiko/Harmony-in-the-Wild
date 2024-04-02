using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmashingRockSpawner : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] private float respawnTime;

    [Header("Needed References")]
    [SerializeField] private GameObject rockPrefab;
    [SerializeField] private GameObject spawnEffect;

    public static SmashingRockSpawner instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("There is more than one Smashing Rock Spawner.");
        }

        instance = this;
    }

    public void SpawnStartingRocks()
    {
        int oreRockIndex = Random.Range(0, transform.childCount);
        Debug.Log("Ore rock index: " + oreRockIndex);
        for(int i = 0; i < transform.childCount; i++)
        {
            Instantiate(spawnEffect, transform.GetChild(i).position, Quaternion.identity);
            GameObject newRock = Instantiate(rockPrefab, transform.GetChild(i));

            if(i == oreRockIndex)
            {
                newRock.GetComponent<Destructible>().IncludeOre();
                Debug.Log("Name of rock with ore: " + newRock.transform.parent);
            }
        }
    }

    public void ResetRocks()
    {
        foreach(Transform child in transform)
        {
            if(child.childCount > 0)
            {
                Destroy(child.transform.GetChild(0).gameObject);
            }
        }

        StartCoroutine(RespawnDelay());
    }

    private IEnumerator RespawnDelay()
    {
        yield return new WaitForSeconds(respawnTime);

        SpawnStartingRocks();
    }

    public void DestroyRocks()
    {
        foreach (Transform child in transform)
        {
            if (child.childCount > 0)
            {
                Instantiate(spawnEffect, child.position, Quaternion.identity);
                Destroy(child.transform.GetChild(0).gameObject);
            }
        }
    }
}
