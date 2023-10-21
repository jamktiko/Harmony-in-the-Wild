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

    private void Start()
    {
        SpawnRock();
    }

    private void SpawnRock()
    {
        Vector3 spawnPosition = new Vector3(Random.Range(minX, maxX), height, Random.Range(minZ, maxX));

        GameObject newRock = Instantiate(rockPrefabs[Random.Range(0, rockPrefabs.Count)], transform);
        newRock.transform.position = spawnPosition;

        StartCoroutine(SpawnPause());
    }

    private IEnumerator SpawnPause()
    {
        yield return new WaitForSeconds(spawnPauseTime);

        SpawnRock();
    }
}
