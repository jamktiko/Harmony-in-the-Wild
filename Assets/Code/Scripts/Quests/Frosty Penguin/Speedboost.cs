using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.VFX;

public class Speedboost : MonoBehaviour
{
    [FormerlySerializedAs("speedIncrease")]
    [Header("Config")]
    [SerializeField] private float _speedIncrease;
    [FormerlySerializedAs("boostVFXDuration")] [SerializeField] private float _boostVFXDuration = 5f;

    [FormerlySerializedAs("speedBoostVFX")]
    [Header("VFX")]
    [SerializeField] private VisualEffect _speedBoostVFX;
    [FormerlySerializedAs("meshTrail")] [SerializeField] internal MeshTrail _meshTrail;
    // Event IDs for the VFX graph events, because we can't directly reference them.
    private int _onEnableSpeedBoostID;
    private int _onDisableSpeedBoostID;

    private void Awake()
    {
        // Get the property IDs for the VFX events as defined in the VFX graph event nodes and set them to the corresponding variables.
        _onEnableSpeedBoostID = Shader.PropertyToID("OnSpeedBoostStart");
        _onDisableSpeedBoostID = Shader.PropertyToID("OnSpeedBoostStop");
    }

    public void IncreasePlayerSpeed(GameObject player)
    {
        player.GetComponent<FoxMovement>().MoveSpeed += _speedIncrease;
        AudioManager.Instance.PlaySound(AudioName.MovementSpeedboost, transform);
        StartCoroutine(SpeedBoostVFXCoroutine(player));
        _meshTrail.ActivateTrail();
    }

    private IEnumerator SpeedBoostVFXCoroutine(GameObject player)
    {
        if (_speedBoostVFX != null)
        {
            Debug.Log("VFX Component found");
            // Trigger the speed boost start VFX using the event ID we created earlier.
            _speedBoostVFX.SendEvent(_onEnableSpeedBoostID);

            // Wait for the duration of the VFX before stopping it.
            yield return new WaitForSeconds(_boostVFXDuration);

            // Trigger the speed boost stop VFX using the event ID we created earlier.
            _speedBoostVFX.SendEvent(_onDisableSpeedBoostID);
        }
    }
}
