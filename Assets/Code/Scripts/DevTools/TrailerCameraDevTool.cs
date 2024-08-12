using UnityEngine;

public class TrailerCameraDevTool : MonoBehaviour
{
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
        // Movement Input
        float moveHorizontal = Input.GetAxis("Horizontal"); // A and D
        float moveVertical = Input.GetAxis("Vertical");     // W and S

        Vector3 movement = transform.forward * moveVertical + transform.right * moveHorizontal;
        transform.position += movement * moveSpeed * Time.deltaTime;

        // Mouse Input
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        // Adjust the rotation based on the mouse movement
        yaw += mouseX;
        pitch -= mouseY;
        pitch = Mathf.Clamp(pitch, -90f, 90f); // Limiting the pitch to avoid flipping

        // Apply rotation to the player
        transform.eulerAngles = new Vector3(pitch, yaw, 0.0f);
    }
}