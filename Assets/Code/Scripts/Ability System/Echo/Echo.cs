using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Echo : MonoBehaviour, IAbility
{
    public static Echo instance;

    [HideInInspector] public bool isEchoing = false;

    [Header("Echo Config")]
    [SerializeField] private LayerMask searchableLayers;
    [SerializeField] private float echoRadius = 9f;
    [SerializeField] private float shockwaveDuration = 1.1f;
    [SerializeField] private GameObject shockwaveEffect;
    [SerializeField] private float timeToTriggerEffectForNextFoundObject = 0.8f;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Debug.LogWarning("There is more than one Echo ability.");
            Destroy(gameObject);
            return;
        }
        instance = this;
    }

    private void Start()
    {
        AbilityManager.instance.RegisterAbility(Abilities.Echo, this);

        AbilityManager.instance.UnlockAbility(Abilities.Echo);
        AbilityManager.instance.ActivateAbilityIfUnlocked(Abilities.Echo);
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
        if (!isEchoing)
        {
            Debug.Log("Echo activated");
        }     
    }

    private void TriggerShockwave()
    {
        Instantiate(shockwaveEffect, transform);

        Invoke(nameof(ShowLocatedObjects), shockwaveDuration);
    }

    private void ShowLocatedObjects()
    {
        Collider[] hitColliders = Physics.OverlapSphere(FoxMovement.instance.transform.position, echoRadius, searchableLayers);

        if(hitColliders.Length > 0)
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

            yield return new WaitForSeconds(timeToTriggerEffectForNextFoundObject);
        }
    }
}