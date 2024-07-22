using PhotoMode;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoClipMovement : MonoBehaviour
{
    [SerializeField]private float horizontalInput;
    [SerializeField]private float verticalInput;
    [SerializeField] private PhotoModeInputs photoModeInputs;
    private void Start()
    {
        photoModeInputs=GetComponentInParent<PhotoModeInputs>();
    }
    // Update is called once per frame
    void Update()
    {
        horizontalInput = photoModeInputs.moveAxisCamera.x;
        verticalInput = photoModeInputs.moveAxisCamera.y;
        transform.position += new Vector3(horizontalInput * Time.unscaledDeltaTime*7, verticalInput * Time.unscaledDeltaTime*7, 0);
    }
}
