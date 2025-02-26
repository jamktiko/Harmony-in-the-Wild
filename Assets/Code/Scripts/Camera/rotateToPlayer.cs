using UnityEngine;
using UnityEngine.Serialization;

public class RotateToPlayer : MonoBehaviour
{
    [FormerlySerializedAs("fox")] [SerializeField] Transform _fox;
    // Update is called once per frame
    void FixedUpdate()
    {
        transform.eulerAngles = new Vector3(-90, 0, _fox.eulerAngles.y);
    }
}
