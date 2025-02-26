using UnityEngine;

public class RespawnObstacle : MonoBehaviour
{
    [Header("Needed References")]
    [SerializeField] private GameObject objectPrefab;

    private void Start()
    {
        PenguinRaceManager.instance.penguinDungeonEvents.onLapInterrupted += RespawnObject;
        PenguinRaceManager.instance.penguinDungeonEvents.onLapFinished += RespawnObject;
    }

    private void OnDisable()
    {
        PenguinRaceManager.instance.penguinDungeonEvents.onLapInterrupted -= RespawnObject;
        PenguinRaceManager.instance.penguinDungeonEvents.onLapFinished -= RespawnObject;
    }

    private void RespawnObject()
    {
        transform.GetChild(0).gameObject.SetActive(true);
    }
}
