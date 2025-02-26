using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

public class SmashingRockSpawner : MonoBehaviour
{
    [FormerlySerializedAs("respawnTime")]
    [Header("Config")]
    [SerializeField] private float _respawnTime;

    [FormerlySerializedAs("rockPrefab")]
    [Header("Needed References")]
    [SerializeField] private GameObject _rockPrefab;
    [FormerlySerializedAs("spawnEffect")] [SerializeField] private GameObject _spawnEffect;

    public static SmashingRockSpawner Instance;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogWarning("There is more than one Smashing Rock Spawner.");
        }

        Instance = this;
    }

    public void SpawnStartingRocks()
    {
        int oreRockIndex = Random.Range(0, transform.childCount);
        Debug.Log("Ore rock index: " + oreRockIndex);
        for (int i = 0; i < transform.childCount; i++)
        {
            Instantiate(_spawnEffect, transform.GetChild(i).position, Quaternion.identity);
            GameObject newRock = Instantiate(_rockPrefab, transform.GetChild(i));

            if (i == oreRockIndex)
            {
                newRock.GetComponent<Destructible>().IncludeOre();
                Debug.Log("Name of rock with ore: " + newRock.transform.parent);
            }
        }
    }

    public void ResetRocks()
    {
        foreach (Transform child in transform)
        {
            if (child.childCount > 0)
            {
                Destroy(child.transform.GetChild(0).gameObject);
            }
        }

        StartCoroutine(RespawnDelay());
    }

    private IEnumerator RespawnDelay()
    {
        yield return new WaitForSeconds(_respawnTime);

        SpawnStartingRocks();
    }

    public void DestroyRocks()
    {
        foreach (Transform child in transform)
        {
            if (child.childCount > 0)
            {
                Instantiate(_spawnEffect, child.position, Quaternion.identity);
                Destroy(child.transform.GetChild(0).gameObject);
            }
        }
    }
}
