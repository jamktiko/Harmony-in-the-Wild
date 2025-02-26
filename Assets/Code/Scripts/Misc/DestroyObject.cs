using UnityEngine;

public class DestroyObject : MonoBehaviour
{
    [SerializeField] private float destructionTime;
    [SerializeField] private GameObject destroyEffect;

    private void OnEnable()
    {
        Destroy(gameObject, destructionTime);
    }

    private void OnDestroy()
    {
        if (destroyEffect != null)
        {
            Instantiate(destroyEffect, transform.position, Quaternion.identity);
        }
    }
}
