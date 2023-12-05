using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [Header("References")]
    public Transform orientation;
    public Transform fox;
    public Transform foxObject;
    public Transform foxMiddle;
    public Rigidbody rb;
    public FoxMovement foxmove;

    public float rotationSpeed;

    [Header("CameraStyles")]
    public Transform TelegrabLookAt;

    public cameraStyle currentStyle;

    [SerializeField] public GameObject freeLookCam;
    [SerializeField] public GameObject telegrabCam;

        public enum cameraStyle 
    {
        Basic,
        Telegrab,
        Topdown
    }
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        foxmove = FindObjectOfType<FoxMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        //rotate orientation
        Vector3 viewDir = fox.position - new Vector3(transform.position.x, fox.position.y, transform.position.z);
        orientation.forward = viewDir.normalized;
    }
    private void FixedUpdate()
    {
        //rotate player object
        if (currentStyle == cameraStyle.Basic)
        {
            float horizontalInput = Input.GetAxis("Horizontal");
            float verticalInput = Input.GetAxis("Vertical");
            Vector3 inputDir = orientation.forward * verticalInput + orientation.right * horizontalInput;
            Vector3 slopeForward = Vector3.ProjectOnPlane(foxObject.forward, foxmove.hit3.normal).normalized;

            if (inputDir != Vector3.zero && !foxmove.OnSlope())
            {
                foxObject.forward = Vector3.Slerp(foxObject.forward, inputDir.normalized, Time.deltaTime * rotationSpeed);
            }
            else if (inputDir != Vector3.zero && foxmove.OnSlope() && slopeForward != Vector3.zero)
            {
                foxObject.forward = Vector3.Slerp(foxObject.forward, inputDir.normalized + slopeForward, Time.deltaTime * rotationSpeed);
            }


        }
        else if (currentStyle == cameraStyle.Telegrab)
        {
            Vector3 dirtoTelegraphLookAt = TelegrabLookAt.position - new Vector3(transform.position.x, TelegrabLookAt.position.y, transform.position.z);
            Vector3 slopeForward = Vector3.ProjectOnPlane(foxObject.forward, foxmove.hit3.normal).normalized;
            if (foxmove.OnSlope())
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
