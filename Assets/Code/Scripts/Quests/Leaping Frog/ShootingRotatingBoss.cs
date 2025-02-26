using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

public class ShootingRotatingBoss : MonoBehaviour
{
    [FormerlySerializedAs("rotationSpeed")]
    [Header("Config")]
    [SerializeField] private float _rotationSpeed;
    [FormerlySerializedAs("shootingChargeTime")]
    [Tooltip("Charging time between triggering shooting and creating projectile")]
    [SerializeField] private float _shootingChargeTime;
    [FormerlySerializedAs("shootingCooldown")] [SerializeField] private float _shootingCooldown;

    [FormerlySerializedAs("projectilePrefab")]
    [Header("Needed References")]
    [SerializeField] private GameObject _projectilePrefab;
    [FormerlySerializedAs("shootingSpot")] [SerializeField] private Transform _shootingSpot;
    [FormerlySerializedAs("instructionPanel")] [SerializeField] private GameObject _instructionPanel;
    [FormerlySerializedAs("player")] [SerializeField] private Transform _player;

    private bool _canShoot = true;
    private AudioSource _audioSource;
    private Coroutine _cooldownCoroutine;
    private Coroutine _initializationCoroutine;

    private void Start()
    {
        //StartCoroutine(ShootingCooldown());
        _audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        transform.Rotate(Vector3.up, _rotationSpeed * Time.deltaTime);

        if (_canShoot && CanSeePlayer() && !_instructionPanel.gameObject.activeInHierarchy)
        {
            _canShoot = false;
            _initializationCoroutine = StartCoroutine(InitializeShooting());
        }
    }

    private bool CanSeePlayer()
    {
        Vector3 currentDirection = _player.position - _shootingSpot.position;
        RaycastHit hit;

        if (Physics.Raycast(_shootingSpot.position, currentDirection, out hit))
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
        _audioSource.Play();

        yield return new WaitForSeconds(_shootingChargeTime);

        ShootProjectile();
    }

    private void ShootProjectile()
    {
        _cooldownCoroutine = StartCoroutine(ShootingCooldown());

        GameObject newProjectile = Instantiate(_projectilePrefab, _shootingSpot);

        newProjectile.GetComponent<Projectile>().InitializeProjectile(_player.position, transform);
    }

    private IEnumerator ShootingCooldown()
    {
        yield return new WaitForSeconds(_shootingCooldown);

        _canShoot = true;
    }

    public void DisableShooting()
    {
        StopAllCoroutines();

        _canShoot = false;
    }
}
