using UnityEngine;
using UnityEngine.Serialization;

public class RotateToPlayer : MonoBehaviour
{
    [FormerlySerializedAs("fox")] [SerializeField]
    private Transform _fox;
    // Update is called once per frame
    private void FixedUpdate()
    {
        transform.eulerAngles = new Vector3(-90, 0, _fox.eulerAngles.y);
    }
}
