using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [Header("References")]
    public Transform orientation;
    public Transform fox;
    public Transform foxObject;
    public Rigidbody rb;

    public float rotationSpeed;

    [Header("CameraStyles")]
    public Transform TelegrabLookAt;

    public cameraStyle currentStyle;

        public enum cameraStyle 
    {
        Basic,
        Telegraph,
        Topdown
    }
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        //rotate orientation
        Vector3 viewDir =fox.position-new Vector3(transform.position.x,fox.position.y,transform.position.z);
        orientation.forward = viewDir.normalized;

        //rotate player object
        if (currentStyle==cameraStyle.Basic)
        {
            float horizontalInput = Input.GetAxis("Horizontal");
            float verticalInput = Input.GetAxis("Vertical");
            Vector3 inputDir = orientation.forward * verticalInput + orientation.right * horizontalInput;

            if (inputDir != Vector3.zero)
            {
                foxObject.forward = Vector3.Slerp(foxObject.forward, inputDir.normalized, Time.deltaTime * rotationSpeed);
            }
        }
        else if (currentStyle==cameraStyle.Telegraph)
        {
            Vector3 dirtoTelegraphLookAt = TelegrabLookAt.position - new Vector3(transform.position.x, TelegrabLookAt.position.y, transform.position.z);
            orientation.forward = dirtoTelegraphLookAt.normalized;

            foxObject.forward = dirtoTelegraphLookAt.normalized;
        }
    }
}
