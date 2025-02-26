using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class RockSpawner : MonoBehaviour
{
    [FormerlySerializedAs("rockPrefabs")]
    [Header("Needed References")]
    [SerializeField] private List<GameObject> _rockPrefabs;

    [FormerlySerializedAs("minX")]
    [Header("Spawn Area Config")]
    [SerializeField] private float _minX;
    [FormerlySerializedAs("maxX")] [SerializeField] private float _maxX;
    [FormerlySerializedAs("minZ")] [SerializeField] private float _minZ;
    [FormerlySerializedAs("maxZ")] [SerializeField] private float _maxZ;
    [FormerlySerializedAs("height")] [SerializeField] private float _height;

    [FormerlySerializedAs("spawnPauseTime")]
    [Header("Spawn Config")]
    [SerializeField] private float _spawnPauseTime;
    [FormerlySerializedAs("canSpawnRocks")] [SerializeField] private bool _canSpawnRocks;

    private void OnEnable()
    {
        PenguinRaceManager.instance.PenguinDungeonEvents.OnLapInterrupted += DisableRockSpawning;
        PenguinRaceManager.instance.PenguinDungeonEvents.OnLapFinished += IncreaseRockSpawning;
    }

    private void OnDisable()
    {
        PenguinRaceManager.instance.PenguinDungeonEvents.OnLapInterrupted -= DisableRockSpawning;
        PenguinRaceManager.instance.PenguinDungeonEvents.OnLapFinished -= IncreaseRockSpawning;
    }

    private void SpawnRock()
    {
        Vector3 spawnPosition = new Vector3(Random.Range(_minX, _maxX), _height, Random.Range(_minZ, _maxZ));

        GameObject newRock = Instantiate(_rockPrefabs[Random.Range(0, _rockPrefabs.Count)], transform);
        newRock.transform.localPosition = spawnPosition;

        StartCoroutine(SpawnPause());
    }

    private IEnumerator SpawnPause()
    {
        yield return new WaitForSeconds(_spawnPauseTime);

        if (_canSpawnRocks)
        {
            SpawnRock();
        }
    }
    public void IncreaseRockSpawning()
    {
        _spawnPauseTime = _spawnPauseTime / 2;
    }

    // ----------------------------------------------------------
    // ENABLING METHODS (called from progress tracking colliders)
    // ----------------------------------------------------------

    public void EnableRockSpawning()
    {
        _canSpawnRocks = true;
        SpawnRock();
    }

    public void DisableRockSpawning()
    {
        _canSpawnRocks = false;
    }
}
