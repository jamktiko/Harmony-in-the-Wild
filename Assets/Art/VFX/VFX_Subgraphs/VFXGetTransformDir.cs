using UnityEngine;
using UnityEngine.VFX;

public class VFXGetTransformDir : MonoBehaviour
{
    // Gets flat transform velocity & directional angle of transform movement for Y rotation of horizontal billboard trails
    public VisualEffect vfx;
    private Vector2 oldPos = Vector3.zero;
    private float angle;

    // Update is called once per frame
    private void Update()
    {
        if (transform.position.x != oldPos.x || transform.position.z != oldPos.y)
        {
            vfx.SetVector2("DirVector", (new Vector2(transform.position.x, transform.position.z) - oldPos).normalized);
            vfx.SetFloat("DirAngle", Vector2.SignedAngle(new Vector2(transform.position.x, transform.position.z) - oldPos, Vector2.right));
            vfx.SetFloat("TransformVelocity", (new Vector2(transform.position.x, transform.position.z) - oldPos).magnitude / Time.deltaTime);
        }
        oldPos = new Vector2(transform.position.x, transform.position.z);
    }
}
