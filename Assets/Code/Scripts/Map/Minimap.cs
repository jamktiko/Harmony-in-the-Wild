using UnityEngine;
using UnityEngine.Serialization;

public class Minimap : MonoBehaviour
{
    [FormerlySerializedAs("minimapTexture")] public RenderTexture MinimapTexture;
    [FormerlySerializedAs("verticalInput")] public float VerticalInput;

    [FormerlySerializedAs("orientation")] public Transform Orientation;

    // Update is called once per frame
    private void Update()
    {
        VerticalInput = Input.GetAxisRaw("Mouse X");
    }
    void LateUpdate()
    {
        Graphics.Blit(null, MinimapTexture);
    }
    private void FixedUpdate()
    {
        //transform.Rotate(Vector3.forward, -verticalInput);
        //transform.eulerAngles = new Vector3(90, 0, orientation.eulerAngles.y);
    }
}
