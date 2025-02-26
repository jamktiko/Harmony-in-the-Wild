using UnityEngine;

public class NoClipMovement : MonoBehaviour
{
    [SerializeField] Vector2 rotation;
    [SerializeField] private float horizontalInput;
    [SerializeField] private float verticalInput;
    [SerializeField] private float upDownInput;
    [SerializeField] private float speed;
    [SerializeField] Rigidbody rb;
    [SerializeField] Transform FpsCamera;
    [SerializeField] private float speed2;

    private void Start()
    {
        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.Locked;
    }
    // Update is called once per frame
    void Update()
    {
        rotation.y += UnityEngine.InputSystem.Mouse.current.delta.ReadValue().x;
        rotation.x += -UnityEngine.InputSystem.Mouse.current.delta.ReadValue().y;
        rotation.x = Mathf.Clamp(rotation.x, -180, 180f);
        horizontalInput = PlayerInputHandler.instance.MoveInput.ReadValue<Vector2>().x;
        verticalInput = PlayerInputHandler.instance.MoveInput.ReadValue<Vector2>().y;
        upDownInput = PlayerInputHandler.instance.JumpInput.ReadValue<float>() > 0 ? PlayerInputHandler.instance.JumpInput.ReadValue<float>() : -PlayerInputHandler.instance.SnowDiveInput.ReadValue<float>();
        speed = PlayerInputHandler.instance.SprintInput.ReadValue<float>() > 0 ? 20f : 10f;
        transform.Translate(new Vector3(horizontalInput * speed * Time.unscaledDeltaTime, upDownInput * Time.unscaledDeltaTime * speed, verticalInput * speed * Time.unscaledDeltaTime));
        transform.eulerAngles = (Vector2)rotation * speed2;
        PauseTimeScale();
    }
    void PauseTimeScale()
    {
        if (PlayerInputHandler.instance.SwitchTimeScale.WasPressedThisFrame())
        {
            Time.timeScale = Time.timeScale == 0 ? 1 : 0;
        }
    }
}
