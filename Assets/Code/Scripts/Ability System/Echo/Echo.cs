using System;
using System.Collections;
using UnityEngine;

public class Echo : MonoBehaviour, IAbility
{
    public static Echo Instance;

    [HideInInspector] public bool _isEchoing = false;

    [Header("Echo Config")]
    [SerializeField] private LayerMask _searchableLayers;
    [SerializeField] private float _echoRadius = 9f;
    [SerializeField] private float _shockwaveDuration = 1.1f;
    [SerializeField] private GameObject _shockwaveEffect;
    [SerializeField] private float _timeToTriggerEffectForNextFoundObject = 0.8f;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Debug.LogWarning("There is more than one Echo ability.");
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        AbilityManager.Instance.RegisterAbility(Abilities.Echo, this);

        AbilityManager.Instance.UnlockAbility(Abilities.Echo);
        AbilityManager.Instance.ActivateAbilityIfUnlocked(Abilities.Echo);
    }

    private void Update()
    {
        if (PlayerInputHandler.instance.UseAbilityInput.WasPressedThisFrame())
        {
            TriggerShockwave();
        }
    }

    public void Activate()
    {
        if (!_isEchoing)
        {
            Debug.Log("Echo activated");
        }
    }

    private void TriggerShockwave()
    {
        Instantiate(_shockwaveEffect, transform);

        Invoke(nameof(ShowLocatedObjects), _shockwaveDuration);
    }

    private void ShowLocatedObjects()
    {
        Collider[] hitColliders = Physics.OverlapSphere(FoxMovement.instance.transform.position, _echoRadius, _searchableLayers);

        if (hitColliders.Length > 0)
        {
            hitColliders = ListObjectsBasedOnDistanceToPlayer(hitColliders);

            StartCoroutine(ActivateFoundObjectEffectsBasedOnDistance(hitColliders));
        }
    }

    private Collider[] ListObjectsBasedOnDistanceToPlayer(Collider[] foundObjects)
    {
        Array.Sort(foundObjects, (firstObject, secondObject) => Vector3.Distance(transform.position, firstObject.transform.position).CompareTo(Vector3.Distance(transform.position, secondObject.transform.position)));

        return foundObjects;
    }

    private IEnumerator ActivateFoundObjectEffectsBasedOnDistance(Collider[] foundObjects)
    {
        foreach (Collider foundObject in foundObjects)
        {
            EchoReceiver echoReceiver = foundObject.transform.parent.GetComponent<EchoReceiver>();

            if (echoReceiver != null)
            {
                echoReceiver.ObjectLocated();
            }

            yield return new WaitForSeconds(_timeToTriggerEffectForNextFoundObject);
        }
    }
}