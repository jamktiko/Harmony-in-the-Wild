using UnityEngine;
using UnityEngine.Serialization;

public class CameraMovement : MonoBehaviour
{
    public enum CameraStyle
    {
        Basic,
        Telegrab,
        Topdown
    }

    [FormerlySerializedAs("orientation")] [Header("References")]
    public Transform Orientation;
    [FormerlySerializedAs("fox")] public Transform Fox;
    [FormerlySerializedAs("foxObject")] public Transform FoxObject;
    [FormerlySerializedAs("foxMiddle")] public Transform FoxMiddle;
    [FormerlySerializedAs("foxBottom")] public Transform FoxBottom;
    [FormerlySerializedAs("foxFront")] public Transform FoxFront;
    [FormerlySerializedAs("rb")] public Rigidbody Rb;
    [FormerlySerializedAs("foxMove")] public FoxMovement FoxMove;

    [FormerlySerializedAs("rotationSpeed")] public float RotationSpeed;

    [Header("CameraStyles")]
    public Transform TelegrabLookAt;

    [FormerlySerializedAs("currentStyle")] public CameraStyle CurrentStyle;

    [FormerlySerializedAs("freeLookCam")] [SerializeField] public GameObject FreeLookCam;
    [FormerlySerializedAs("telegrabCam")] [SerializeField] public GameObject TelegrabCam;

    [FormerlySerializedAs("horizontalInput")] [SerializeField]
    private float _horizontalInput;
    [FormerlySerializedAs("verticalInput")] [SerializeField]
    private float _verticalInput;
    [FormerlySerializedAs("mouseInput")] [SerializeField]
    private Vector2 _mouseInput;


    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        FoxMove = FindObjectOfType<FoxMovement>();
        FoxObject = transform.parent.GetComponentInChildren<Animator>().transform;
    }

    private void Update()
    {
        //rotate orientation
        Vector3 viewDir = Fox.position - new Vector3(transform.position.x, Fox.position.y, transform.position.z);
        Orientation.forward = viewDir.normalized;
    }

    private void FixedUpdate()
    {
        //rotate player object
        if (CurrentStyle == CameraStyle.Basic)
        {

            //foxObject.rotation = Quaternion.Euler(Mathf.Clamp(foxObject.rotation.eulerAngles.x, -20, 20), foxObject.rotation.eulerAngles.y, foxObject.rotation.eulerAngles.z);
            float horizontalInput = PlayerInputHandler.Instance.MoveInput.ReadValue<Vector2>().x;
            float verticalInput = PlayerInputHandler.Instance.MoveInput.ReadValue<Vector2>().y;
            Vector3 inputDir = Orientation.forward * verticalInput + Orientation.right * horizontalInput;
            Vector3 slopeForward = Vector3.ProjectOnPlane(FoxObject.forward, FoxMove.SlopeHit.normal).normalized;

            if (inputDir != Vector3.zero && !FoxMove.IsOnSlope())
            {
                FoxObject.forward = Vector3.Slerp(FoxObject.forward, inputDir.normalized, Time.deltaTime * RotationSpeed);
                FoxObject.eulerAngles = new Vector3(0, FoxObject.rotation.eulerAngles.y, FoxObject.rotation.eulerAngles.z);
            }
            else if (inputDir != Vector3.zero && FoxMove.IsOnSlope() && slopeForward != Vector3.zero)
            {
                FoxObject.forward = Vector3.Slerp(FoxObject.forward, inputDir.normalized + slopeForward, Time.deltaTime * RotationSpeed);
                slopeForward = Vector3.zero;

            }
            else if (!FoxMove.IsOnSlope())
            {
                if (FoxObject.rotation.eulerAngles.x < 305)
                {
                    FoxObject.rotation = Quaternion.Euler(360, FoxObject.rotation.eulerAngles.y, FoxObject.rotation.eulerAngles.z);
                }
            }


        }
        else if (CurrentStyle == CameraStyle.Telegrab)
        {
            Vector3 dirtoTelegraphLookAt = TelegrabLookAt.position - new Vector3(transform.position.x, TelegrabLookAt.position.y, transform.position.z);
            Vector3 slopeForward = Vector3.ProjectOnPlane(FoxObject.forward, FoxMove.SlopeHit.normal).normalized;
            if (FoxMove.IsOnSlope())
            {
                Orientation.forward = Vector3.Slerp(Orientation.forward, dirtoTelegraphLookAt.normalized + slopeForward, Time.deltaTime * RotationSpeed);
                FoxObject.forward = Vector3.Slerp(FoxObject.forward, dirtoTelegraphLookAt.normalized + slopeForward, Time.deltaTime * RotationSpeed);
            }
            else
            {
                Orientation.forward = dirtoTelegraphLookAt.normalized;
                FoxObject.forward = dirtoTelegraphLookAt.normalized;
            }
        }
    }

    private void UpdateCharacterBones()
    {
        RaycastHit clavicleHit;
        RaycastHit buttHit;

        if (Physics.Raycast(FoxFront.position, Vector3.down, out clavicleHit, 10) && Physics.Raycast(FoxBottom.position, Vector3.down, out buttHit, 10))
        {

        }



    }
}
