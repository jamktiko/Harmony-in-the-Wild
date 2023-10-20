using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] private float moveSpeed;

    private Vector3 targetPosition;

    private void Update()
    {
        if(targetPosition != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

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
        if (other.gameObject.CompareTag("Player"))
        {
            other.GetComponent<HitCounter>().TakeHit();
            Destroy(gameObject);
        }
    }
}
