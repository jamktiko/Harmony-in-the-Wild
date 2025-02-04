using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlidingOnIceAudioTrigger : MonoBehaviour
{
    [SerializeField] private float maxDelayTime = 5f;
    [SerializeField] private float minDelayTime = 3f;

    private void Start()
    {
        CheckForSlipperyAudio();
    }

    private void CheckForSlipperyAudio()
    {
        float horizontalInput = PlayerInputHandler.instance.MoveInput.ReadValue<Vector2>().x;
        float verticalInput = PlayerInputHandler.instance.MoveInput.ReadValue<Vector2>().y;

        if (horizontalInput > 0 || verticalInput > 0)
        {
            AudioManager.instance.PlaySound(AudioName.Movement_SlidingOnIce, transform);
        }

        StartCoroutine(DelayBetweenSlipperyAudio());
    }

    private IEnumerator DelayBetweenSlipperyAudio()
    {
        yield return new WaitForSeconds(Random.Range(minDelayTime, maxDelayTime));

        CheckForSlipperyAudio();
    }
}