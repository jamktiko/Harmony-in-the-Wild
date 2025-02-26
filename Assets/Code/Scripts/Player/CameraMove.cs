using UnityEngine;
using UnityEngine.Serialization;

public class CameraMove : MonoBehaviour
{
    private const float _yMin = 10.0f;
    private const float _yMax = 50.0f;

    [FormerlySerializedAs("lookAt")] public Transform LookAt;

    public Transform Player;

    [FormerlySerializedAs("distance")] public float Distance = 10.0f;
    [FormerlySerializedAs("currentX")] [SerializeField] private float _currentX = 0.0f;
    [FormerlySerializedAs("currentY")] [SerializeField] private float _currentY = 0.0f;
    [FormerlySerializedAs("sensivity")] public float Sensivity = 100f;
    [SerializeField] public float X = 0.0f;
    [SerializeField] public float Y = 0.0f;

    [FormerlySerializedAs("animator")] [SerializeField] Animator _animator;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        _animator = GetComponentInParent<Animator>();
    }

    void LateUpdate()
    {

        _currentX += Input.GetAxis("Mouse X") * Sensivity * Time.deltaTime;
        _currentY -= -Input.GetAxis("Mouse Y") * Sensivity * Time.deltaTime;
        X = Input.GetAxis("Mouse X") * Time.deltaTime;
        Y = Input.GetAxis("Mouse Y") * Time.deltaTime;

        _currentY = Mathf.Clamp(_currentY, _yMin, _yMax);

        Vector3 direction = new Vector3(0, 0, -Distance);
        Quaternion rotation = Quaternion.Euler(_currentY, _currentX, 0);
        transform.position = LookAt.position + rotation * direction;

        transform.LookAt(LookAt.position);

    }
}