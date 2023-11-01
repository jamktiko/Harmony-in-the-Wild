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
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Vector3 inputDir = orientation.forward*verticalInput+orientation.right*horizontalInput;

        if (inputDir!=Vector3.zero)
        {
            foxObject.forward = Vector3.Slerp(foxObject.forward, inputDir.normalized, Time.deltaTime * rotationSpeed);
        }
    }
}
