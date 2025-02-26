using UnityEngine;
using UnityEngine.Serialization;

public class DestroyObject : MonoBehaviour
{
    [FormerlySerializedAs("destructionTime")] [SerializeField] private float _destructionTime;
    [FormerlySerializedAs("destroyEffect")] [SerializeField] private GameObject _destroyEffect;

    private void OnEnable()
    {
        Destroy(gameObject, _destructionTime);
    }

    private void OnDestroy()
    {
        if (_destroyEffect != null)
        {
            Instantiate(_destroyEffect, transform.position, Quaternion.identity);
        }
    }
}
