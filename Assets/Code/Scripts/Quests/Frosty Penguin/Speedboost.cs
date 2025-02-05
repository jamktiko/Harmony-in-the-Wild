using System;
using System.Collections;
using UnityEngine;
using UnityEngine.VFX;

public class Speedboost : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] private float speedIncrease;
    [SerializeField] private float boostVFXDuration = 5f;

    [Header("VFX")]
    [SerializeField] private VisualEffect speedBoostVFX;
    [SerializeField] internal MeshTrail meshTrail;
    // Event IDs for the VFX graph events, because we can't directly reference them.
    private int onEnableSpeedBoostID;
    private int onDisableSpeedBoostID;

    private void Awake()
    {
        // Get the property IDs for the VFX events as defined in the VFX graph event nodes and set them to the corresponding variables.
        onEnableSpeedBoostID = Shader.PropertyToID("OnSpeedBoostStart");
        onDisableSpeedBoostID = Shader.PropertyToID("OnSpeedBoostStop");
    }

    public void IncreasePlayerSpeed(GameObject player)
    {
        player.GetComponent<FoxMovement>().moveSpeed += speedIncrease;
        AudioManager.instance.PlaySound(AudioName.Movement_Speedboost, transform);
        StartCoroutine(SpeedBoostVFXCoroutine(player));
        meshTrail.ActivateTrail();
    }

    private IEnumerator SpeedBoostVFXCoroutine(GameObject player)
    {
        if (speedBoostVFX != null)
        {
            Debug.Log("VFX Component found");
            // Trigger the speed boost start VFX using the event ID we created earlier.
            speedBoostVFX.SendEvent(onEnableSpeedBoostID);

            // Wait for the duration of the VFX before stopping it.
            yield return new WaitForSeconds(boostVFXDuration);

            // Trigger the speed boost stop VFX using the event ID we created earlier.
            speedBoostVFX.SendEvent(onDisableSpeedBoostID);
        }
    }
}
