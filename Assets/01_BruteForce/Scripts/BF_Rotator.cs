using UnityEngine;

public class BF_Rotator : MonoBehaviour
{
    public float RotSpeed = 1;

    private void Update()
    {
        this.transform.Rotate(new Vector3(0, RotSpeed * Time.deltaTime, 0), Space.World);
    }
}
