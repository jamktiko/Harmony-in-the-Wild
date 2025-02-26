using UnityEngine;
using UnityEngine.Serialization;

public class Projectile : MonoBehaviour
{
    [FormerlySerializedAs("projectileSpeed")]
    [Header("Config")]
    [SerializeField] private float _projectileSpeed;

    private Transform _bossTransform;
    private Vector3 _targetPosition;

    private void Start()
    {
        Invoke("DelayDestroy", 6.5f);
    }

    private void Update()
    {
        if (_targetPosition != null)
        {
            if (transform != null)
            {

                transform.position = Vector3.MoveTowards(transform.position, _targetPosition, _projectileSpeed * Time.deltaTime);
                transform.rotation = Quaternion.Euler(0.0f, 0.0f, _bossTransform.rotation.z * -1.0f);

                if (transform.position == _targetPosition)
                {
                    Destroy(gameObject);
                }
            }
        }

    }

    public void InitializeProjectile(Vector3 playerLocation, Transform parentTransform)
    {
        _targetPosition = playerLocation;
        _bossTransform = parentTransform;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Trigger"))
        {
            other.GetComponentInParent<HitCounter>().TakeHit(false);
            Destroy(gameObject);
        }
    }

    private void DelayDestroy()
    {
        Destroy(gameObject);
    }
}
