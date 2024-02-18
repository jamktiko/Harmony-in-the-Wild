using UnityEngine;

public class Projectile : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] private float projectileSpeed;

    private Vector3 targetPosition;

    private void Update()
    {
        if(targetPosition != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, projectileSpeed * Time.deltaTime);

            if(transform.position == targetPosition)
            {
                Destroy(gameObject);
            }
        }
    }

    public void InitializeProjectile(Vector3 playerLocation)
    {
        targetPosition = playerLocation;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Trigger"))
        {
            other.GetComponentInParent<HitCounter>().TakeHit(false);
            Destroy(gameObject);
        }
    }
}
