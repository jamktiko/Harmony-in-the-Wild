using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlidingOnIceAudioTrigger : MonoBehaviour
{
    [SerializeField] private float maxDelayTime = 5f;
    [SerializeField] private float minDelayTime = 3f;

    private float currentTime = 0f;
    private float targetTime;
    private bool canPlayAudio = false;

    private void Start()
    {
        SetTargetTimeForNewCheck();
        CheckForSlipperyAudio();
    }

    private void OnEnable()
    {
        PenguinRaceManager.instance.penguinDungeonEvents.onRaceFinished += DisableAudio;
        GameEventsManager.instance.uiEvents.OnHideInstructionPanel += EnableAudio;
        GameEventsManager.instance.playerEvents.OnToggleInputActions += ToggleAudioOnPause;
    }

    private void OnDisable()
    {
        PenguinRaceManager.instance.penguinDungeonEvents.onRaceFinished -= DisableAudio;
        GameEventsManager.instance.uiEvents.OnHideInstructionPanel -= EnableAudio;
        GameEventsManager.instance.playerEvents.OnToggleInputActions -= ToggleAudioOnPause;
    }

    private void Update()
    {
        if (canPlayAudio)
        {
            if (currentTime < targetTime)
            {
                currentTime += Time.deltaTime;
            }

            else
            {
                CheckForSlipperyAudio();
            }
        }
    }

    private void CheckForSlipperyAudio()
    {
        float horizontalInput = PlayerInputHandler.instance.MoveInput.ReadValue<Vector2>().x;
        float verticalInput = PlayerInputHandler.instance.MoveInput.ReadValue<Vector2>().y;

        if (horizontalInput > 0 && verticalInput == 0)
        {
            AudioManager.instance.PlaySound(AudioName.Movement_SlidingOnIce, transform);
        }

        currentTime = 0f;
        SetTargetTimeForNewCheck();
    }

    private void SetTargetTimeForNewCheck()
    {
        targetTime = Random.Range(minDelayTime, maxDelayTime);
    }

    private void EnableAudio()
    {
        canPlayAudio = true;
    }

    private void DisableAudio()
    {
        canPlayAudio = false;
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