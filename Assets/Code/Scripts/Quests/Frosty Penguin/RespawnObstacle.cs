using UnityEngine;
using UnityEngine.Serialization;

public class RespawnObstacle : MonoBehaviour
{
    [FormerlySerializedAs("objectPrefab")]
    [Header("Needed References")]
    [SerializeField] private GameObject _objectPrefab;

    private void Start()
    {
        PenguinRaceManager.instance.PenguinDungeonEvents.OnLapInterrupted += RespawnObject;
        PenguinRaceManager.instance.PenguinDungeonEvents.OnLapFinished += RespawnObject;
    }

    private void OnDisable()
    {
        PenguinRaceManager.instance.PenguinDungeonEvents.OnLapInterrupted -= RespawnObject;
        PenguinRaceManager.instance.PenguinDungeonEvents.OnLapFinished -= RespawnObject;
    }

    private void RespawnObject()
    {
        transform.GetChild(0).gameObject.SetActive(true);
    }
}
