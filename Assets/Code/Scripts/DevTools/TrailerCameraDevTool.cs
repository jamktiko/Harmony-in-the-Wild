using UnityEngine;

public class TrailerCameraDevTool : MonoBehaviour
{
#if DEBUG
    public float moveSpeed = 5f;
    public float mouseSensitivity = 2f;

    private float pitch = 0f;  // Vertical rotation (up and down)
    private float yaw = 0f;    // Horizontal rotation (left and right)

    void Start()
    {
        // Lock the cursor to the game window
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        float horizontalInput = PlayerInputHandler.instance.MoveInput.ReadValue<Vector2>().x;
        float verticalInput = PlayerInputHandler.instance.MoveInput.ReadValue<Vector2>().y;

        Vector3 movement = transform.forward * verticalInput + transform.right * horizontalInput;
        transform.position += movement * moveSpeed * Time.deltaTime;

        // Mouse Input
        float mouseX = PlayerInputHandler.instance.LookInput.ReadValue<Vector2>().x * mouseSensitivity;
        float mouseY = PlayerInputHandler.instance.LookInput.ReadValue<Vector2>().y * mouseSensitivity;

        // Adjust the rotation based on the mouse movement
        yaw += mouseX;
        pitch -= mouseY;
        pitch = Mathf.Clamp(pitch, -90f, 90f); // Limiting the pitch to avoid flipping

        // Apply rotation to the player
        transform.eulerAngles = new Vector3(pitch, yaw, 0.0f);

        if (PlayerInputHandler.instance.DebugIncreaseTrailerCameraSpeed.WasPressedThisFrame())
        {
            moveSpeed += 2f;
        }

        else if (PlayerInputHandler.instance.DebugDecreaseTrailerCameraSpeed.WasPressedThisFrame())
        {
            moveSpeed -= 2f;
        }
    }
#endif
}