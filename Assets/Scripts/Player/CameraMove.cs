using UnityEngine;

public class CameraMove : MonoBehaviour
{
    private const float YMin = 10.0f;
    private const float YMax = 50.0f;

    public Transform lookAt;

    public Transform Player;

    public float distance = 10.0f;
    [SerializeField] private float currentX = 0.0f;
    [SerializeField]private float currentY = 0.0f;
    public float sensivity = 100f;
    [SerializeField] public float X = 0.0f;
    [SerializeField] public float Y = 0.0f;

    [SerializeField] Animator animator;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        animator=GetComponentInParent<Animator>();
    }

    void LateUpdate()
    {

        currentX += Input.GetAxis("Mouse X") * sensivity * Time.deltaTime;
        currentY -= -Input.GetAxis("Mouse Y") * sensivity * Time.deltaTime;
        X= Input.GetAxis("Mouse X") * Time.deltaTime;
        Y= Input.GetAxis("Mouse Y")  * Time.deltaTime;
        
        currentY = Mathf.Clamp(currentY, YMin, YMax);

        Vector3 Direction = new Vector3(0, 0, -distance);
        Quaternion rotation = Quaternion.Euler(currentY, currentX, 0);
        transform.position = lookAt.position + rotation * Direction;

        transform.LookAt(lookAt.position);

    }
}