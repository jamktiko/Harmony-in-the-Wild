using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingRotatingBoss : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float shootingCooldown;

    [Header("Needed References")]
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform shootingSpot;
    [SerializeField] private Transform player;

    private bool canShoot;

    private void Start()
    {
        StartCoroutine(ShootingCooldown());
    }

    private void Update()
    {
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);

        if (canShoot && CanSeePlayer())
        {
            ShootProjectile();
        }
    }

    private bool CanSeePlayer()
    {
        Vector3 currentDirection = player.position - shootingSpot.position;
        RaycastHit hit;

        if(Physics.Raycast(shootingSpot.position, currentDirection, out hit))
        {
            if (hit.collider.CompareTag("Player"))
            {
                return true;
            }
        }

        return false;
    }

    private void ShootProjectile()
    {
        canShoot = false;
        StartCoroutine(ShootingCooldown());

        GameObject newProjectile = Instantiate(projectilePrefab, shootingSpot);

        newProjectile.GetComponent<Projectile>().InitializeProjectile(player.position);
    }

    private IEnumerator ShootingCooldown()
    {
        yield return new WaitForSeconds(shootingCooldown);

        canShoot = true;
    }
}
