using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockSpawner : MonoBehaviour
{
    [Header("Needed References")]
    [SerializeField] private List<GameObject> rockPrefabs;

    [Header("Spawn Area Config")]
    [SerializeField] private float minX;
    [SerializeField] private float maxX;
    [SerializeField] private float minZ;
    [SerializeField] private float maxZ;
    [SerializeField] private float height;

    [Header("Spawn Config")]
    [SerializeField] private float spawnPauseTime;
    [SerializeField] private bool canSpawnRocks;

    private void OnEnable()
    {
        PenguinRaceManager.instance.penguinDungeonEvents.onLapInterrupted += DisableRockSpawning;
        PenguinRaceManager.instance.penguinDungeonEvents.onLapFinished += IncreaseRockSpawning;
    }

    private void OnDisable()
    {
        PenguinRaceManager.instance.penguinDungeonEvents.onLapInterrupted -= DisableRockSpawning;
        PenguinRaceManager.instance.penguinDungeonEvents.onLapFinished -= IncreaseRockSpawning;
    }

    private void SpawnRock()
    {
        Vector3 spawnPosition = new Vector3(Random.Range(minX, maxX), height, Random.Range(minZ, maxZ));

        GameObject newRock = Instantiate(rockPrefabs[Random.Range(0, rockPrefabs.Count)], transform);
        newRock.transform.localPosition = spawnPosition;

        StartCoroutine(SpawnPause());
    }

    private IEnumerator SpawnPause()
    {
        yield return new WaitForSeconds(spawnPauseTime);

        if (canSpawnRocks)
        {
            SpawnRock();
        }
    }
    public void IncreaseRockSpawning()
    {
        spawnPauseTime = spawnPauseTime * 0.3f;
    }

    // ----------------------------------------------------------
    // ENABLING METHODS (called from progress tracking colliders)
    // ----------------------------------------------------------

    public void EnableRockSpawning()
    {
        canSpawnRocks = true;
        SpawnRock();
    }

    public void DisableRockSpawning()
    {
        canSpawnRocks = false;
    }
}
