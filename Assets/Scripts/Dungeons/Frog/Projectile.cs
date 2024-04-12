using UnityEngine;

public class Projectile : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] private float projectileSpeed;

    private Transform bossTransform;
    private Vector3 targetPosition;

    private void Start()
    {
        Invoke("DelayDestroy", 6.5f);
    }

    private void Update()
    {
        if(targetPosition != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, projectileSpeed * Time.deltaTime);
            transform.rotation = Quaternion.Euler(0.0f, 0.0f, bossTransform.rotation.z * -1.0f);

            if (transform.position == targetPosition)
            {
                Destroy(gameObject);
            }
        }

    }

    public void InitializeProjectile(Vector3 playerLocation, Transform parentTransform)
    {
        targetPosition = playerLocation;
        parentTransform = bossTransform;
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
