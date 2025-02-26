using UnityEngine;
using UnityEngine.Serialization;

public class TrailerCameraDevTool : MonoBehaviour
{
#if DEBUG
    [FormerlySerializedAs("moveSpeed")] public float MoveSpeed = 5f;
    [FormerlySerializedAs("mouseSensitivity")] public float MouseSensitivity = 2f;

    private float _pitch = 0f;  // Vertical rotation (up and down)
    private float _yaw = 0f;    // Horizontal rotation (left and right)

    void Start()
    {
        // Lock the cursor to the game window
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        float horizontalInput = PlayerInputHandler.Instance.MoveInput.ReadValue<Vector2>().x;
        float verticalInput = PlayerInputHandler.Instance.MoveInput.ReadValue<Vector2>().y;

        Vector3 movement = transform.forward * verticalInput + transform.right * horizontalInput;
        transform.position += movement * MoveSpeed * Time.deltaTime;

        // Mouse Input
        float mouseX = PlayerInputHandler.Instance.LookInput.ReadValue<Vector2>().x * MouseSensitivity;
        float mouseY = PlayerInputHandler.Instance.LookInput.ReadValue<Vector2>().y * MouseSensitivity;

        // Adjust the rotation based on the mouse movement
        _yaw += mouseX;
        _pitch -= mouseY;
        _pitch = Mathf.Clamp(_pitch, -90f, 90f); // Limiting the pitch to avoid flipping

        // Apply rotation to the player
        transform.eulerAngles = new Vector3(_pitch, _yaw, 0.0f);

        if (PlayerInputHandler.Instance.DebugIncreaseTrailerCameraSpeed.WasPressedThisFrame())
        {
            MoveSpeed += 2f;
        }

        else if (PlayerInputHandler.Instance.DebugDecreaseTrailerCameraSpeed.WasPressedThisFrame())
        {
            MoveSpeed -= 2f;
        }
    }
#endif
}