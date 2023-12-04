using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minimap : MonoBehaviour
{
    public RenderTexture minimapTexture;
    public float verticalInput;

    public Transform orientation;

    // Update is called once per frame
    private void Update()
    {
       verticalInput=Input.GetAxisRaw("Mouse X");
    }
    void LateUpdate()
    {
        Graphics.Blit(null, minimapTexture);
    }
    private void FixedUpdate()
    {
        //transform.Rotate(Vector3.forward, -verticalInput);
        //transform.eulerAngles = new Vector3(90, 0, orientation.eulerAngles.y);
    }
}
