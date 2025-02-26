using UnityEngine;

public class GetPlayerPosition : MonoBehaviour
{
    [SerializeField] Transform fox;

    void FixedUpdate()
    {
        transform.SetLocalPositionAndRotation(new Vector3(fox.position.x, 0, fox.position.z), Quaternion.Euler(-90, 0, fox.eulerAngles.y));
    }
}
