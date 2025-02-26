using UnityEngine;
using UnityEngine.Serialization;

public class NoClipMovement : MonoBehaviour
{
    [FormerlySerializedAs("rotation")] [SerializeField] Vector2 _rotation;
    [FormerlySerializedAs("horizontalInput")] [SerializeField] private float _horizontalInput;
    [FormerlySerializedAs("verticalInput")] [SerializeField] private float _verticalInput;
    [FormerlySerializedAs("upDownInput")] [SerializeField] private float _upDownInput;
    [FormerlySerializedAs("speed")] [SerializeField] private float _speed;
    [FormerlySerializedAs("rb")] [SerializeField] Rigidbody _rb;
    [FormerlySerializedAs("FpsCamera")] [SerializeField] Transform _fpsCamera;
    [FormerlySerializedAs("speed2")] [SerializeField] private float _speed2;

    private void Start()
    {
        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.Locked;
    }
    // Update is called once per frame
    void Update()
    {
        _rotation.y += UnityEngine.InputSystem.Mouse.current.delta.ReadValue().x;
        _rotation.x += -UnityEngine.InputSystem.Mouse.current.delta.ReadValue().y;
        _rotation.x = Mathf.Clamp(_rotation.x, -180, 180f);
        _horizontalInput = PlayerInputHandler.Instance.MoveInput.ReadValue<Vector2>().x;
        _verticalInput = PlayerInputHandler.Instance.MoveInput.ReadValue<Vector2>().y;
        _upDownInput = PlayerInputHandler.Instance.JumpInput.ReadValue<float>() > 0 ? PlayerInputHandler.Instance.JumpInput.ReadValue<float>() : -PlayerInputHandler.Instance.SnowDiveInput.ReadValue<float>();
        _speed = PlayerInputHandler.Instance.SprintInput.ReadValue<float>() > 0 ? 20f : 10f;
        transform.Translate(new Vector3(_horizontalInput * _speed * Time.unscaledDeltaTime, _upDownInput * Time.unscaledDeltaTime * _speed, _verticalInput * _speed * Time.unscaledDeltaTime));
        transform.eulerAngles = (Vector2)_rotation * _speed2;
        PauseTimeScale();
    }
    void PauseTimeScale()
    {
        if (PlayerInputHandler.Instance.SwitchTimeScale.WasPressedThisFrame())
        {
            Time.timeScale = Time.timeScale == 0 ? 1 : 0;
        }
    }
}
