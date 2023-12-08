using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Speedboost : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] private float speedIncrease;
    [SerializeField] AudioSource speedBoostAudio;

    public void IncreasePlayerSpeed(GameObject player)
    {
        speedBoostAudio.Play();
        player.GetComponent<FoxMovement>().moveSpeed += speedIncrease;
    }
}
