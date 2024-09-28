using System.Collections;
using UnityEngine;

public class ShootingRotatingBoss : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] private float rotationSpeed;
    [Tooltip("Charging time between triggering shooting and creating projectile")]
    [SerializeField] private float shootingChargeTime;
    [SerializeField] private float shootingCooldown;

    [Header("Needed References")]
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform shootingSpot;
    [SerializeField] private GameObject instructionPanel;
    [SerializeField] private Transform player;

    private bool canShoot = true;
    private AudioSource audioSource;
    private Coroutine cooldownCoroutine;
    private Coroutine initializationCoroutine;

    private void Start()
    {
        //StartCoroutine(ShootingCooldown());
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);

        if (canShoot && CanSeePlayer() && !instructionPanel.gameObject.activeInHierarchy)
        {
            canShoot = false;
            initializationCoroutine = StartCoroutine(InitializeShooting());
        }
    }

    private bool CanSeePlayer()
    {
        Vector3 currentDirection = player.position - shootingSpot.position;
        RaycastHit hit;

        if(Physics.Raycast(shootingSpot.position, currentDirection, out hit))
        {
            if (hit.collider.CompareTag("Trigger"))
            {
                return true;
            }
        }

        return false;
    }

    private IEnumerator InitializeShooting()
    {
        audioSource.Play();

        yield return new WaitForSeconds(shootingChargeTime);

        ShootProjectile();
    }

    private void ShootProjectile()
    {
        cooldownCoroutine = StartCoroutine(ShootingCooldown());

        GameObject newProjectile = Instantiate(projectilePrefab, shootingSpot);

        newProjectile.GetComponent<Projectile>().InitializeProjectile(player.position, transform);
    }

    private IEnumerator ShootingCooldown()
    {
        yield return new WaitForSeconds(shootingCooldown);

        canShoot = true;
    }

    public void DisableShooting()
    {
        StopAllCoroutines();

        canShoot = false;
    }
}
