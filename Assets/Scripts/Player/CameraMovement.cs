using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public enum CameraStyle 
    {
        Basic,
        Telegrab,
        Topdown
    }

    [Header("References")]
    public Transform orientation;
    public Transform fox;
    public Transform foxObject;
    public Transform foxMiddle;
    public Rigidbody rb;
    public FoxMovement foxMove;

    public float rotationSpeed;

    [Header("CameraStyles")]
    public Transform TelegrabLookAt;

    public CameraStyle currentStyle;

    [SerializeField] public GameObject freeLookCam;
    [SerializeField] public GameObject telegrabCam;

    [SerializeField] float horizontalInput;
    [SerializeField] float verticalInput;
    [SerializeField] Vector2 mouseInput;


    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        foxMove = FindObjectOfType<FoxMovement>();
        foxObject = transform.parent.GetComponentInChildren<Animator>().transform;
    }

    void Update()
    {
        //rotate orientation
        Vector3 viewDir = fox.position - new Vector3(transform.position.x, fox.position.y, transform.position.z);
        orientation.forward = viewDir.normalized;
    }

    private void FixedUpdate()
    {
        //rotate player object
        if (currentStyle == CameraStyle.Basic)
        {
            if (foxObject.rotation.eulerAngles.x < 305)
            {
                foxObject.rotation = Quaternion.Euler(360, foxObject.rotation.eulerAngles.y, foxObject.rotation.eulerAngles.z);
            }
            //foxObject.rotation = Quaternion.Euler(Mathf.Clamp(foxObject.rotation.eulerAngles.x, -20, 20), foxObject.rotation.eulerAngles.y, foxObject.rotation.eulerAngles.z);
            float horizontalInput = PlayerInputHandler.instance.MoveInput.ReadValue<Vector2>().x;
            float verticalInput = PlayerInputHandler.instance.MoveInput.ReadValue<Vector2>().y;
            Vector3 inputDir = orientation.forward * verticalInput + orientation.right * horizontalInput;
            Vector3 slopeForward = Vector3.ProjectOnPlane(foxObject.forward, foxMove.hit3.normal).normalized;

            if (inputDir != Vector3.zero && !foxMove.IsOnSlope())
            {
                foxObject.forward = Vector3.Slerp(foxObject.forward, inputDir.normalized, Time.deltaTime * rotationSpeed);
                foxObject.eulerAngles=new Vector3 (0, foxObject.rotation.eulerAngles.y, foxObject.rotation.eulerAngles.z);
            }
            else if (inputDir != Vector3.zero && foxMove.IsOnSlope() && slopeForward != Vector3.zero)
            {
                foxObject.forward = Vector3.Slerp(foxObject.forward, inputDir.normalized + slopeForward, Time.deltaTime * rotationSpeed);
                slopeForward = Vector3.zero;

            }
            

        }
        else if (currentStyle == CameraStyle.Telegrab)
        {
            Vector3 dirtoTelegraphLookAt = TelegrabLookAt.position - new Vector3(transform.position.x, TelegrabLookAt.position.y, transform.position.z);
            Vector3 slopeForward = Vector3.ProjectOnPlane(foxObject.forward, foxMove.hit3.normal).normalized;
            if (foxMove.IsOnSlope())
            {
                orientation.forward =Vector3.Slerp(orientation.forward, dirtoTelegraphLookAt.normalized + slopeForward, Time.deltaTime * rotationSpeed);
                foxObject.forward = Vector3.Slerp(foxObject.forward, dirtoTelegraphLookAt.normalized + slopeForward, Time.deltaTime * rotationSpeed);
            }
            else
            {
                orientation.forward = dirtoTelegraphLookAt.normalized;
                foxObject.forward = dirtoTelegraphLookAt.normalized;
            }
        }
    }
}
