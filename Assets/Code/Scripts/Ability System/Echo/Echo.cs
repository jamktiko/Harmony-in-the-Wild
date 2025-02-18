using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Echo : MonoBehaviour, IAbility
{
    public static Echo instance;

    [HideInInspector] public bool isEchoing = false;

    [Header("Echo Config")]
    [SerializeField] private LayerMask searchableLayers;
    [SerializeField] private float echoRadius = 9f;
    [SerializeField] private float shockwaveDuration = 1.1f;
    [SerializeField] private GameObject shockwaveEffect;

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
            foreach(Collider foundObject in hitColliders)
            {
                EchoReceiver echoReceiver = foundObject.transform.parent.GetComponent<EchoReceiver>();

                if(echoReceiver != null)
                {
                    echoReceiver.ObjectLocated();
                }
            }
        }
    }
}