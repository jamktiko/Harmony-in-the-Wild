using UnityEngine;
using UnityEngine.Serialization;

public class SlidingOnIceAudioTrigger : MonoBehaviour
{
    [FormerlySerializedAs("maxDelayTime")] [SerializeField] private float _maxDelayTime = 5f;
    [FormerlySerializedAs("minDelayTime")] [SerializeField] private float _minDelayTime = 3f;

    private float _currentTime = 0f;
    private float _targetTime;
    private bool _canPlayAudio = false;

    private void Start()
    {
        SetTargetTimeForNewCheck();
        CheckForSlipperyAudio();
    }

    private void OnEnable()
    {
        PenguinRaceManager.instance.PenguinDungeonEvents.OnRaceFinished += DisableAudio;
        GameEventsManager.instance.UIEvents.OnHideInstructionPanel += EnableAudio;
        GameEventsManager.instance.PlayerEvents.OnToggleInputActions += ToggleAudioOnPause;
    }

    private void OnDisable()
    {
        PenguinRaceManager.instance.PenguinDungeonEvents.OnRaceFinished -= DisableAudio;
        GameEventsManager.instance.UIEvents.OnHideInstructionPanel -= EnableAudio;
        GameEventsManager.instance.PlayerEvents.OnToggleInputActions -= ToggleAudioOnPause;
    }

    private void Update()
    {
        if (_canPlayAudio)
        {
            if (_currentTime < _targetTime)
            {
                _currentTime += Time.deltaTime;
            }

            else
            {
                CheckForSlipperyAudio();
            }
        }
    }

    private void CheckForSlipperyAudio()
    {
        float horizontalInput = PlayerInputHandler.Instance.MoveInput.ReadValue<Vector2>().x;
        float verticalInput = PlayerInputHandler.Instance.MoveInput.ReadValue<Vector2>().y;

        Debug.Log($"{horizontalInput}, {verticalInput}");

        if ((horizontalInput != 0 || verticalInput != 0) && FoxMovement.Instance.IsGrounded())
        {
            AudioManager.Instance.PlaySound(AudioName.MovementSlidingOnIce, transform);
        }

        _currentTime = 0f;
        SetTargetTimeForNewCheck();
    }

    private void SetTargetTimeForNewCheck()
    {
        _targetTime = Random.Range(_minDelayTime, _maxDelayTime);
    }

    private void EnableAudio()
    {
        _canPlayAudio = true;
    }

    private void DisableAudio()
    {
        _canPlayAudio = false;
    }

    private void ToggleAudioOnPause(bool audioOn)
    {
        if (audioOn)
        {
            EnableAudio();
        }

        else
        {
            DisableAudio();
        }
    }
}