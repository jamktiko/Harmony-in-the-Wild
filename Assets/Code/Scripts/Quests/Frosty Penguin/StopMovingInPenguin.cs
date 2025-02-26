using UnityEngine;
using UnityEngine.Serialization;

public class StopMovingInPenguin : MonoBehaviour
{
    private FoxMovement _foxMovement;
    [FormerlySerializedAs("cameraMovement")] [SerializeField] private CameraMovement _cameraMovement;
    [FormerlySerializedAs("camera")] [SerializeField] private GameObject _camera;

    private void Start()
    {
        _foxMovement = GetComponent<FoxMovement>();
        PenguinRaceManager.instance.PenguinDungeonEvents.OnTimeRanOut += DisableMovement;
    }

    private void OnDisable()
    {
        PenguinRaceManager.instance.PenguinDungeonEvents.OnTimeRanOut -= DisableMovement;
    }

    private void DisableMovement()
    {
        _foxMovement.enabled = false;
        _cameraMovement.enabled = false;
        _camera.gameObject.SetActive(false);
    }
}
