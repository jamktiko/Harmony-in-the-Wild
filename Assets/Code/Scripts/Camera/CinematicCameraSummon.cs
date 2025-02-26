using UnityEngine;
using UnityEngine.Serialization;

public class CinematicCameraSummon : MonoBehaviour
{
    [FormerlySerializedAs("cinCam")] [SerializeField] GameObject _cinCam;
    [FormerlySerializedAs("instance")] [SerializeField] GameObject _instance;
    private void Update()
    {
        //StartCinematicCamera();
    }
    void StartCinematicCamera()
    {
        if (PlayerInputHandler.Instance.CinematicCamera.WasPressedThisFrame() && _instance == null)
        {
            _instance = Instantiate(_cinCam, transform.position, transform.rotation);
        }
        else if (PlayerInputHandler.Instance.CinematicCamera.WasPressedThisFrame() && _instance != null)
        {
            DestroyImmediate(_instance);
            _instance = null;
        }
    }
}
